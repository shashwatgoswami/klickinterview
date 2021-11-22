using Klick.SimpleUserDatabase.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Klick.SimpleUserDatabase.Service
{
    /// <summary>
    /// user specific implementation of the service
    /// </summary>
    internal sealed class UserService : BaseService<User, int>
    {
        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="serviceProvider">dependency container</param>
        internal UserService(IServiceProvider serviceProvider): base(serviceProvider)
        {
        }
    }
}
