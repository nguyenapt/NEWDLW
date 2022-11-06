using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dlw.EpiBase.Content.Infrastructure.Extensions;
using EPiServer.Data.Configuration;
using EPiServer.Data.Configuration.Transform.Internal;
using EPiServer.Data.Internal;
using EPiServer.Data.Providers.Internal;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Framework;
using EPiServer.Framework.Configuration;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    /// <summary>
    /// Module that ensures the database.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class EnsureDatabaseInitializableModule : IInitializableModule, IConfigurableModule
    {
        public const string CmsCoreScript = "Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase.Scripts.EPiServer.Cms.Core.sql";
        public const string CommerceCoreScript = "Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase.Scripts.EPiServer.Commerce.Core.sql";

        public const string CommerceConnectionStringName = "EcfSqlConnection";

        private IEnsureDatabaseAction _ensureDatabaseAction;

        private static bool _databasesCreated = false;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            EnsureDatabasesFallback(context);
        }

        public void Initialize(InitializationEngine context)
        {
            // Disabled until we can wire this module before DataInitialization where other db work (schema creation / update) is done
            #pragma warning disable 162 // keep code alive (code refactorings, epi updates)
            if (false)
            {
                _ensureDatabaseAction = new ApiEnsureDatabaseAction();

                var dataAccessOptions = context.Locate.Advanced.GetInstance<DataAccessOptions>();
                var databaseConnectionResolver = context.Locate.Advanced.GetInstance<IDatabaseConnectionResolver>();

                if (!dataAccessOptions.CreateDatabaseSchema) return;

                var defaultConnectionString = databaseConnectionResolver.Resolve();

                EnsureDatabases(defaultConnectionString.ConnectionString, CommerceConnectionStringName);
            }
            #pragma warning restore 162

            // workaround through IConfigurableModule.ConfigureContainer()

            ClearIndex(context);
        }

        private void ClearIndex(InitializationEngine context)
        {
            if (!_databasesCreated) return;

             var searchClient = context.Locate.Advanced.GetInstance<IClient>();

            // new database, get index in sync -> clear
            searchClient.DeleteByQuery(new WildcardQuery("_Type", "*"), null);
        }

        private void EnsureDatabasesFallback(ServiceConfigurationContext context)
        {
            _ensureDatabaseAction = new FallbackEnsureDatabaseAction();

            var dataAccessOptions = GetAndEnsureDataAccessOptions(context);

            if (!dataAccessOptions.CreateDatabaseSchema) return;

            EnsureDatabases(dataAccessOptions.DefaultConnectionStringName, CommerceConnectionStringName);
        }

        private void EnsureDatabases(string cmsConnectionStringName, string commerceConnectionStringName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var cmsDatabaseCreated = _ensureDatabaseAction.Ensure(cmsConnectionStringName);
            if (cmsDatabaseCreated && _ensureDatabaseAction.IsEmpty(cmsConnectionStringName))
            {
                ExecuteScript(assembly, CmsCoreScript, cmsConnectionStringName);

                _databasesCreated = true;
            }

            var commerceDatabaseCreated = _ensureDatabaseAction.Ensure(commerceConnectionStringName);
            if (commerceDatabaseCreated && _ensureDatabaseAction.IsEmpty(commerceConnectionStringName))
            {
                ExecuteScript(assembly, CommerceCoreScript, commerceConnectionStringName);

                _databasesCreated = true;
            }
        }

        private void ExecuteScript(Assembly assembly, string resourceName, string connectionStringName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
                using (var connection = new SqlConnection(connectionStringSettings.ConnectionString))
                {
                    connection.ExecuteBatchNonQuery(result);
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            // nothing to do
        }

        // DataAccessOptions resolved at this stage is not fully ensured.
        // This triggers this process partially, just for local usage.
        private DataAccessOptions GetAndEnsureDataAccessOptions(ServiceConfigurationContext context)
        {
            var epiDataStoreSection = context.StructureMap().GetInstance<EPiServerDataStoreSection>();
            var epiFrameworkSection = context.StructureMap().GetInstance<EPiServerFrameworkSection>();

            var dataAccessOptions = context.StructureMap().GetInstance<DataAccessOptions>();

            var configTransform = new WrappedDatabaseConfigurationTransform(epiDataStoreSection, epiFrameworkSection, dataAccessOptions, null);
            configTransform.TriggerCreateDataAccessOptionFromConfig();

            return dataAccessOptions;
        }

        // Wrapper to expose protected property, this to avoid triggering ServiceLocator calls and related initializations which will occur on a later stage by epi.
        private class WrappedDatabaseConfigurationTransform : DatabaseConfigurationTransform
        {
            public WrappedDatabaseConfigurationTransform(EPiServerDataStoreSection dataSection, EPiServerFrameworkSection frameworkSection, DataAccessOptions dataOptions, IServiceLocator provider)
                : base(dataSection, frameworkSection, dataOptions, provider)
            { }

            public void TriggerCreateDataAccessOptionFromConfig()
            {
                base.CreateDataAccessOptionFromConfig();
            }
        }
    }
}