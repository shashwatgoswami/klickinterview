namespace Klick.SimpleUserDatabase.DataAccess.DbClient
{
	/// <summary>
	/// Klick database context for Simple User Database
	/// </summary>
    public sealed class KlickDbContext : DbContext
    {
		/// <summary>
		/// Intializes the class
		/// </summary>
		/// <param name="connectionString">connection string</param>
		/// <param name="providerName">provide name default is SQL Server client</param>
		public KlickDbContext(string connectionString, string providerName = "System.Data.SqlClient") : base(connectionString, providerName)
		{
		}
	}
}
