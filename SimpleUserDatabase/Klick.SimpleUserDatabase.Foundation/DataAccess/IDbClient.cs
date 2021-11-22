using System;
using System.Data;
using System.Data.Common;

namespace Klick.SimpleUserDatabase.Foundation.DataAccess
{
    /// <summary>
    /// This interface represents the contract for the database clients
    /// </summary>
    public interface IDbClient
    {
        /// <summary>
        /// This method creates database connection
        /// </summary>
        /// <returns>Database connection</returns>
        DbConnection CreateConnection();

        /// <summary>
        /// Creates database command
        /// </summary>
        /// <returns>Database command</returns>
        DbCommand CreateCommand();

        /// <summary>
        /// Creates SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="direction">Parameter direction</param>
        /// <param name="databaseType">Database type</param>
        /// <returns>Database parameter</returns>
        IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction, DbType databaseType);

        /// <summary>
        /// Creates SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <returns>Database parameter</returns>
        IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection);
    }
}
