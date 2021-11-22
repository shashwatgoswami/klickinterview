using System;
using System.Data.Common;

namespace Klick.SimpleUserDatabase.Foundation.DataAccess
{
    /// <summary>
    /// database extension methods
    /// </summary>
    public static class DbExtensions
    {
		/// <summary>
		/// Gets the value of column from data reader.
		/// </summary>
		/// <typeparam name="TValue">Column value type.</typeparam>
		/// <param name="reader">DB data reader.</param>
		/// <param name="columnName">Column name.</param>
		/// <returns>Column value if column is present otherwise returns default.</retu
		public static TValue GetValue<TValue>(this DbDataReader reader, string columnName)
		{
			if (reader != null && reader[columnName] != DBNull.Value)
			{
				return (TValue)reader[columnName];
			}
			else
			{
				return default;
			}
		}
	}
}
