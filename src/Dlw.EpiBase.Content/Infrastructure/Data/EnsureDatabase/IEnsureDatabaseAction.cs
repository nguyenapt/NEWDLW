namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    public interface IEnsureDatabaseAction
    {
        /// <summary>
        /// Ensure the database from connectionstring.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns>Returns true when database is created.</returns>
        bool Ensure(string connectionStringName);

        /// <summary>
        /// Checks if the database is empty.
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        bool IsEmpty(string connectionName);
    }
}