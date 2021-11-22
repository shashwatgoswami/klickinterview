using Klick.SimpleUserDatabase.DataAccess.Repositories;
using Klick.SimpleUserDatabase.Foundation.Model;
using Klick.SimpleUserDatabase.Foundation.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.Service
{
    /// <summary>
    /// base service class having basic service implementation
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    /// <typeparam name="U">Primary key</typeparam>
    internal abstract class BaseService<T, U> : IService<T, U> where T : IModel<U>
    {
        /// <summary>
        /// data repository
        /// </summary>
        protected readonly IRepository<T, U> repository;

        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="serviceProvider">dependency container</param>
        internal BaseService(IServiceProvider serviceProvider)
        {
            this.repository = RepositoryFactory.Create<T, U>(serviceProvider);
        }

        /// <summary>
        /// Creates the entity
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Primary key of the entity</returns>
        public async virtual Task<U> Create(T entity)
        {
            if (entity != null)
            {
                return await this.repository.Create(entity);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Gets the entity
        /// </summary>
        /// <param name="id">primary key of entity</param>
        /// <returns>entity</returns>
        public async virtual Task<T> GetByID(U id)
        {
            return await this.repository.GetByID(id);
        }

        /// <summary>
        /// searches based on the entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> Search(T entity)
        {
            if (entity != null)
            {
                return await this.repository.Search(entity);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        public async virtual Task Update(T entity)
        {
            if (entity != null)
            {
                await this.repository.Update(entity);
            }
        }
    }
}
