using Klick.SimpleUserDatabase.Foundation.Model;
using Klick.SimpleUserDatabase.Models;
using System;

namespace Klick.SimpleUserDatabase.DataAccess.Repositories
{
    /// <summary>
    /// Factory class to create a repository
    /// </summary>
    public class RepositoryFactory
    {
        /// <summary>
        /// creates a repository object based on model
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <typeparam name="U">Primary Key</typeparam>
        /// <param name="serviceProvider">Service Provider for Dependency Injection</param>
        /// <returns>repository</returns>
        public static IRepository<T, U> Create<T, U>(IServiceProvider serviceProvider) where T: IModel<U>
        {
            if (typeof(T).Name == typeof(User).Name)
            {
                return ((IRepository<T, U>)(new UserRepository(serviceProvider) as IRepository<User, int>));
            }

            return null;
        }
    }
}
