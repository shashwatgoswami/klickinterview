using Klick.SimpleUserDatabase.Foundation.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.DataAccess.Repositories
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Entity object</typeparam>
    /// <typeparam name="U">Primary key</typeparam>
    public interface IRepository<T, U> where T : IModel<U>
    {
        /// <summary>
        /// Creates the entity
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Primary key of the entity</returns>
        Task<U> Create(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        Task Update(T entity);

        /// <summary>
        /// Gets the entity
        /// </summary>
        /// <param name="id">primary key of entity</param>
        /// <returns>entity</returns>
        Task<T> GetByID(U id);

        /// <summary>
        /// searches based on the entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Search(T entity);
    }
}
