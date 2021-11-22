using System;

namespace Klick.SimpleUserDatabase.Foundation.Model
{
    /// <summary>
    /// base model
    /// </summary>
    /// <typeparam name="T">primary key</typeparam>
    public abstract class BaseModel<T> : IModel<T>
    {
        /// <summary>
        /// primary key
        /// </summary>
        public T ID { get; set; }

        /// <summary>
        /// record deleted flag
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// date time when record was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// date time when the record was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
