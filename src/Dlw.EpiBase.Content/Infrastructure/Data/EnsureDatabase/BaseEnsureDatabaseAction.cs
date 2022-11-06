using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    public abstract class BaseEnsureDatabaseAction : IEnsureDatabaseAction
    {
        protected const string CreateDbScript = "CREATE DATABASE \"{0}\"";
        protected const string CheckDbScript = "SELECT CAST(CASE count(*) WHEN 1 THEN 1 ELSE 0 END AS BIT) FROM sys.databases WHERE name = '{0}'";
        protected const string IsDbEmptyScript = "SELECT CAST(CASE count(*) WHEN 0 THEN 1 ELSE 0 END AS BIT) FROM sys.tables";

        /// <summary>
        /// Ensure database.
        /// </summary>
        /// <returns>Returns true when new database is created during this step.</returns>
        public abstract bool Ensure(string connectionStringName);

        public virtual bool IsEmpty(string connectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];

            bool isEmpty;
            using (var connection = new SqlConnection(connectionString.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = IsDbEmptyScript;
                    isEmpty = (bool)command.ExecuteScalar();
                }
            } 

            return isEmpty;
        }

        /// <summary>
        /// Checks if the database exists.
        /// </summary>
        /// <returns>Returns true if the database needs to be created.</returns>
        public virtual bool CreateDatabase(string connectionStringName)
        {
            var masterConnectionString = ConfigurationManager.ConnectionStrings["Master"];
            if (masterConnectionString == null)
            {
                throw new Exception($"No 'Master' connection string set. Required to ensure databases.");
            }

            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];

            // connection string not found, so db not required
            if (connectionString == null) return false;

            var dbName = GetDatabaseName(connectionString.ConnectionString);

            bool dbExists;
            using (var connection = new SqlConnection(masterConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(CheckDbScript, dbName);
                    dbExists = (bool)command.ExecuteScalar();
                }
            }

            return !dbExists;
        }

        protected string GetDatabaseName(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            return builder.InitialCatalog;
        }
    }
}