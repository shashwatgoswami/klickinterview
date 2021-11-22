using Klick.SimpleUserDatabase.DataAccess.DbClient;
using Klick.SimpleUserDatabase.Foundation.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.DataAccess.Repositories
{
    public abstract class BaseRepository<T, U> : IRepository<T, U> where T : IModel<U>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T,U}"/> class.
        /// </summary>
        /// <param name="connectionStringName">Connection string name</param>
        public BaseRepository(IServiceProvider services)
        {
            IConfiguration config = (IConfiguration)services.GetService(typeof(IConfiguration));
            string connectionString = config.GetConnectionString("KlickDatabaseConnectionString");
            this.DataContext = new KlickDbContext(connectionString);
        }

        /// <summary>
        /// Gets or sets database context
        /// </summary>
        protected KlickDbContext DataContext { get; set; }

        /// <summary>
        /// Creates the entity
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Primary key of the entity</returns>
        public abstract Task<U> Create(T entity);

        /// <summary>
        /// Gets the entity
        /// </summary>
        /// <param name="id">primary key of entity</param>
        /// <returns>entity</returns>
        public abstract Task<T> GetByID(U id);

        /// <summary>
        /// searches based on the entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract Task<IEnumerable<T>> Search(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        public abstract Task Update(T entity);

        /// <summary>
        /// Adds parameter to the list of database data parameter
        /// </summary>
        /// <param name="list">List of database data parameters</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter direction</param>
        /// <param name="databaseType">Database type</param>
        public void AddParameter(IList<IDbDataParameter> list, string parameterName, object parameterValue, ParameterDirection parameterDirection, DbType databaseType)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            list.Add(DataContext.CreateParameter(parameterName, parameterValue, parameterDirection, databaseType));
        }
    }
}
