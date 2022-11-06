using System;
using System.IO;
using System.Linq;
using EPiServer.Data.Internal;
using EPiServer.Data.SchemaUpdates;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    public class ApiEnsureDatabaseAction : BaseEnsureDatabaseAction
    {
        private DataAccessOptions _dataAccessOptions;
        private ScriptExecutor _scriptExecutor;

        public ApiEnsureDatabaseAction()
        {
            _dataAccessOptions = ServiceLocator.Current.GetInstance<DataAccessOptions>();
            _scriptExecutor = ServiceLocator.Current.GetInstance<ScriptExecutor>();
        }

        public override bool Ensure(string connectionStringName)
        {
            if (!CreateDatabase(connectionStringName)) return false;

            var masterConnectionString = _dataAccessOptions.ConnectionStringOptions.SingleOrDefault(x => x.Name.ToLowerInvariant() == "master");
            if (masterConnectionString == null)
            {
                throw new Exception($"No 'Master' connection string set. Required to ensure databases.");
            }

            var dbName = GetDatabaseName(connectionStringName);

            var dbScript = string.Format(CreateDbScript, dbName);

            using (var stream = GenerateStreamFromString(dbScript))
            {
                _scriptExecutor.ExecuteScript(masterConnectionString.ConnectionString, stream);
            }

            return true;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(s);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }
    }
}