using Klick.SimpleUserDatabase.Foundation.DataAccess;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Klick.SimpleUserDatabase.DataAccess.DbClient
{
    /// <summary>
    /// SQL Server client implementation
    /// </summary>
    public sealed class SqlServerClient : IDbClient
    {
        /// <summary>
        /// connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public SqlServerClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates database command
        /// </summary>
        /// <returns>Database command</returns>
        public DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// This method creates database connection
        /// </summary>
        /// <returns>Database connection</retur
        public DbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Creates SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="direction">Parameter direction</param>
        /// <param name="databaseType">Database type</param>
        /// <returns>Database parameter</returns>
        public IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction, DbType databaseType)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Value = parameterValue,
                Direction = direction,
                DbType = databaseType
            };
        }

        /// <summary>
        /// Creates SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <returns>Database parameter</returns>
        public IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Value = parameterValue,
                Direction = parameterDirection,
                SqlDbType = SqlDbType.Structured
            };
        }
    }
}
