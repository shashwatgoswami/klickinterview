using Klick.SimpleUserDatabase.Foundation.Service;
using Klick.SimpleUserDatabase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.Service
{
    /// <summary>
    /// Facade for the managing the user specific operations
    /// </summary>
    public sealed class UserServiceManager
    {
        /// <summary>
        /// user service
        /// </summary>
        private readonly IService<User, int> userService;

        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="serviceProvider">dependency container</param>
        public UserServiceManager(IServiceProvider serviceProvider)
        {
            userService = new UserService(serviceProvider);
        }

        /// <summary>
        /// creates a new user and returns the ID
        /// </summary>
        /// <param name="entity">user object</param>
        /// <returns>Primary key identification</returns>
        public async Task<int> CreateNewUser(User entity)
        {
            return await this.userService.Create(entity);
        }

        /// <summary>
        /// gets an existing user based on ID
        /// </summary>
        /// <param name="userID">Primary key identification</param>
        /// <returns>user object</returns>
        public async Task<User> GetExistingUser(int userID)
        {
            return await this.userService.GetByID(userID);
        }

        /// <summary>
        /// updates existing user
        /// </summary>
        /// <param name="entity">user object</param>
        /// <returns></returns>
        public async Task UpdateExistingUser(User entity)
        {
            await this.userService.Update(entity);
        }

        /// <summary>
        /// activates the user
        /// </summary>
        /// <param name="userID">Primary key identification</param>
        /// <returns></returns>
        public async Task ActivateUser(int userID)
        {
            User user = await this.userService.GetByID(userID);
            user.State = UserState.Active;
            await this.userService.Update(user);
        }

        /// <summary>
        /// deactivates the user
        /// </summary>
        /// <param name="userID">Primary key identification</param>
        /// <returns></returns>
        public async Task DeactivateUser(int userID)
        {
            User user = await this.userService.GetByID(userID);
            user.State = UserState.Inactive;
            await this.userService.Update(user);
        }

        /// <summary>
        /// searches based on the user object
        /// </summary>
        /// <param name="entity">user criteria</param>
        /// <returns>collection of users</returns>
        public async Task<IEnumerable<User>> Search(User entity)
        {
            return await this.userService.Search(entity);
        }
    }
}
