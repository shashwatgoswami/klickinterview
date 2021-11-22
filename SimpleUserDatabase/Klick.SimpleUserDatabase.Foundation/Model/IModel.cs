using System;
using System.Collections.Generic;
using System.Text;

namespace Klick.SimpleUserDatabase.Foundation.Model
{
    /// <summary>
    /// model interface
    /// </summary>
    /// <typeparam name="T">Primary Key</typeparam>
    public interface IModel<T>
    {
        /// <summary>
        /// primary of the model
        /// </summary>
        T ID { get; set; }
    }
}
