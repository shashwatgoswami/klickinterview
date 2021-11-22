using Klick.SimpleUserDatabase.Foundation.DataAccess;
using System;

namespace Klick.SimpleUserDatabase.DataAccess.DbClient
{
    /// <summary>
    /// factory to create database client
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// SQL database provider
        /// </summary>
        private readonly string _providerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbFactory"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string name</param>
        /// <param name="providerName">Provider name</param>
        public DbFactory(string connectionString, string providerName)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                ConnectionString = connectionString;
                _providerName = providerName;
            }
            else
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// This method implements the Factory pattern and creates the database client based on the
        /// SQL provider
        /// </summary>
        /// <returns>Database connection</returns>
        public IDbClient CreateDbClient()
        {
            IDbClient client;
            switch (_providerName)
            {
                case "System.Data.SqlClient":
                    client = new SqlServerClient(ConnectionString);
                    break;
                default:
                    client = null;
                    break;
            }

            return client;
        }
    }
}
