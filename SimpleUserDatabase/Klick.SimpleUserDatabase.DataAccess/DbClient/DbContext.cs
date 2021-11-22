using Klick.SimpleUserDatabase.Foundation.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.DataAccess.DbClient
{
    /// <summary>
    /// base class, providing functions for database context
    /// </summary>
    public class DbContext
    {
        /// <summary>
        /// factory to create database client
        /// </summary>
        private readonly DbFactory _databaseFactory;

        /// <summary>
        /// database client
        /// </summary>
        private readonly IDbClient _databaseClient;

        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="connectionString">connection string</param>
        /// <param name="providerName">provider name, like SQL Server, Oracle</param>
        public DbContext(string connectionString, string providerName)
        {
            _databaseFactory = new DbFactory(connectionString, providerName);
            _databaseClient = _databaseFactory.CreateDbClient();
        }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString => _databaseFactory.ConnectionString;

        /// <summary>
        /// Adds SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <param name="databaseType">Database type</param>
        /// <returns>Database parameter</returns>
        public virtual IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection, DbType databaseType)
        {
            if (parameterValue == null)
            {
                return _databaseClient.CreateParameter(parameterName, DBNull.Value, parameterDirection, databaseType);
            }
            else
            {
                return _databaseClient.CreateParameter(parameterName, parameterValue, parameterDirection, databaseType);
            }
        }

        /// <summary>
        /// Adds SQL parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <returns>Database parameter</returns>
        public virtual IDbDataParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection)
        {
            return _databaseClient.CreateParameter(parameterName, parameterValue, parameterDirection);
        }

        /// <summary>
        /// Executes stored procedure asynchronously
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns>Task object</re
        public virtual async Task ExecuteSPNonQueryAsync(string storedProcedureName, IList<IDbDataParameter> parameters)
        {
            await CommandContainerAsync(CommandType.StoredProcedure, storedProcedureName, parameters, async (command) =>
            {
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes reader asynchronously
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="parameters">Parameters collection</param>
        /// <param name="action">Function delegate</param>
        /// <param name="commandTimeout">optional timeout duration</param>
        /// <returns>Generic Type object</returns>
        public virtual async Task<T> ExecuteSPReaderAsync<T>(string storedProcedureName, IList<IDbDataParameter> parameters, Func<DbDataReader, T> action, int? commandTimeout = null)
        {
            T output = default;
            await CommandContainerAsync(CommandType.StoredProcedure, storedProcedureName, parameters, async (command) =>
            {
                if (commandTimeout != null && commandTimeout.HasValue)
                {
                    command.CommandTimeout = commandTimeout.Value;
                }

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(true);
                output = action(reader);
            }).ConfigureAwait(true);
            return output;
        }

        /// <summary>
        /// contains the command in proper using statements to
        /// dispose of resources after use
        /// </summary>
        /// <param name="commandType">command type</param>
        /// <param name="commandText">command text</param>
        /// <param name="parameters">collection of parameters</param>
        /// <param name="action">action to perform with the command</param>
        /// <returns>async task</returns>
        private async Task CommandContainerAsync(CommandType commandType, string commandText, IList<IDbDataParameter> parameters, Func<DbCommand, Task> action)
        {
            string validatedCommandText = ValidateCommandText(commandType, commandText);
            if (!string.IsNullOrEmpty(validatedCommandText))
            {
                using (DbConnection connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    using (DbCommand command = _databaseClient.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = commandType;
                        command.CommandText = validatedCommandText;
                        if (parameters != null)
                        {
                            AddParametersToCommand(command, parameters);
                        }
                        await action(command).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Adds parameters to SQL command
        /// </summary>
        /// <param name="command">SQL Command</param>
        /// <param name="parameters">Collection of parameters</param>
        private static void AddParametersToCommand(IDbCommand command, IList<IDbDataParameter> parameters)
        {
            foreach (IDbDataParameter param in parameters)
            {
                command.Parameters.Add(param);
            }
        }

        /// <summary>
        /// validates the command text
        /// </summary>
        /// <param name="commandType">type of command</param>
        /// <param name="commandText">command text</param>
        /// <returns>validated command</returns>
        private static string ValidateCommandText(CommandType commandType, string commandText)
        {
            if (commandType == CommandType.Text)
            {
                return ValidateCommandTextForSqlInjection(commandText);
            }
            else
            {
                return commandText;
            }
        }

        /// <summary>
        /// validates the command text by checking for SQL injections
        /// </summary>
        /// <param name="commandText">command text</param>
        /// <returns>validated command</returns>
        private static string ValidateCommandTextForSqlInjection(string commandText)
        {
            string[] sqlCheckList =
            {
                "--", ";--", ";", "/*", "*/", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin",
                "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute",
                "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update"
            };

            string[] commandWords = commandText.Split(' ');
            IEnumerable<string> injectionKeywords = commandWords.Intersect(sqlCheckList);
            if (injectionKeywords.Any())
            {
                return null;
            }
            else
            {
                return commandText;
            }
        }
    }
}
