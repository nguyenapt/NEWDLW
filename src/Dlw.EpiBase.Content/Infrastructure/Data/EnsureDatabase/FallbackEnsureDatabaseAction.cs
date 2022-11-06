using System.Configuration;
using System.Data.SqlClient;

namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    public class FallbackEnsureDatabaseAction : BaseEnsureDatabaseAction
    {
        public override bool Ensure(string connectionStringName)
        {
            if (!CreateDatabase(connectionStringName)) return false;

            var masterConnectionString = ConfigurationManager.ConnectionStrings["Master"];
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            var dbName = GetDatabaseName(connectionString.ConnectionString);

            CreateDatabaseFallback(masterConnectionString.ConnectionString, dbName);

            return true;
        }

        private void CreateDatabaseFallback(string masterConnectionString, string dbName)
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(CreateDbScript, dbName);
                    command.CommandTimeout = 180;

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}