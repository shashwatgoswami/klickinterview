using Klick.SimpleUserDatabase.Foundation.DataAccess;
using Klick.SimpleUserDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Klick.SimpleUserDatabase.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of user repository
    /// </summary>
    public sealed class UserRepository : BaseRepository<User, int>
    {
        /// <summary>
        /// initializes the class
        /// </summary>
        /// <param name="serviceProvider">dependency container</param>
        public UserRepository(IServiceProvider serviceProvider): base(serviceProvider)
        {
        }

        /// <summary>
        /// Creates the entity
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Primary key of the entity</returns>
        public async override Task<int> Create(User entity)
        {
            string commandName = "[dbo].[createUser]";
            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            DataTable tvp = new DataTable();
            AddParameter(paramList, "@name", entity.Name, ParameterDirection.Input, DbType.String);
            AddParameter(paramList, "@emailAddress", entity.EmailAddress, ParameterDirection.Input, DbType.String);
            AddParameter(paramList, "@provinceID", entity.Province != null ? entity.Province.ID : 0, ParameterDirection.Input, DbType.Int32);
            AddParameter(paramList, "@userState", (int)entity.State, ParameterDirection.Input, DbType.Int32);

            if (entity.Medications != null && entity.Medications.Any())
            {
                tvp.Columns.Add(new DataColumn("ID", typeof(int)));
                foreach (int id in entity.Medications.Select(m => m.ID))
                    tvp.Rows.Add(id);
                IDbDataParameter tabelValueParameter = this.DataContext.CreateParameter("@medicationList", tvp, ParameterDirection.Input);
                paramList.Add(tabelValueParameter);
            }

            IDbDataParameter newUserID = this.DataContext.CreateParameter("@userID", null, ParameterDirection.Output, DbType.Int32);
            paramList.Add(newUserID);
            await this.DataContext.ExecuteSPNonQueryAsync(commandName, paramList);
            return (int)newUserID.Value;
        }

        /// <summary>
        /// Gets the entity
        /// </summary>
        /// <param name="id">primary key of entity</param>
        /// <returns>entity</returns>
        public async override Task<User> GetByID(int id)
        {
            string commandName = "[dbo].[getUser]";
            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            AddParameter(paramList, "@userID", id, ParameterDirection.Input, DbType.Int32);
            return await this.DataContext.ExecuteSPReaderAsync<User>(commandName, paramList, MapUserFromReader);
        }

        /// <summary>
        /// searches based on the entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>collection of entities</returns>
        public async override Task<IEnumerable<User>> Search(User entity)
        {
            string commandName = "[dbo].[searchUser]";
            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            DataTable tvp = new DataTable();
            if (entity.ID > 0)
            {
                AddParameter(paramList, "@userID", entity.ID, ParameterDirection.Input, DbType.Int32);
            }
            else
            {
                AddParameter(paramList, "@userID", DBNull.Value, ParameterDirection.Input, DbType.Int32);
            }

            if (!string.IsNullOrEmpty(entity.Name))
            {
                AddParameter(paramList, "@name", entity.Name, ParameterDirection.Input, DbType.String);
            }
            else
            {
                AddParameter(paramList, "@name", DBNull.Value, ParameterDirection.Input, DbType.String);
            }

            if (!string.IsNullOrEmpty(entity.EmailAddress))
            {
                AddParameter(paramList, "@emailAddress", entity.EmailAddress, ParameterDirection.Input, DbType.String);
            }
            else
            {
                AddParameter(paramList, "@emailAddress", DBNull.Value, ParameterDirection.Input, DbType.String);
            }

            if (entity.Province != null && entity.Province.ID > 0)
            {
                AddParameter(paramList, "@provinceID", entity.Province.ID, ParameterDirection.Input, DbType.Int32);
            }
            else
            {
                AddParameter(paramList, "@provinceID", DBNull.Value, ParameterDirection.Input, DbType.Int32);
            }

            if ((int)entity.State > 0)
            {
                AddParameter(paramList, "@userState", (int)entity.State, ParameterDirection.Input, DbType.Int32);
            }
            else
            {
                AddParameter(paramList, "@userState", DBNull.Value, ParameterDirection.Input, DbType.Int32);
            }

            if (entity.Medications != null && entity.Medications.Any())
            {
                tvp.Columns.Add(new DataColumn("ID", typeof(int)));
                foreach (int id in entity.Medications.Select(m => m.ID))
                    tvp.Rows.Add(id);
                IDbDataParameter tabelValueParameter = this.DataContext.CreateParameter("@medicationList", tvp, ParameterDirection.Input);
                paramList.Add(tabelValueParameter);
            }

            IEnumerable<User> users = await this.DataContext.ExecuteSPReaderAsync<IEnumerable<User>>(commandName, paramList, MapUsersFromReader);
            await this.SetUserMedications(users);
            return users;
        }

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        public async override Task Update(User entity)
        {
            string commandName = "[dbo].[updateUser]";
            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            DataTable tvp = new DataTable();
            AddParameter(paramList, "@userID", entity.ID, ParameterDirection.Input, DbType.Int32);
            AddParameter(paramList, "@name", entity.Name, ParameterDirection.Input, DbType.String);
            AddParameter(paramList, "@emailAddress", entity.EmailAddress, ParameterDirection.Input, DbType.String);
            AddParameter(paramList, "@provinceID", entity.Province != null ? entity.Province.ID : 0, ParameterDirection.Input, DbType.Int32);
            AddParameter(paramList, "@userState", (int)entity.State, ParameterDirection.Input, DbType.Int32);

            if (entity.Medications != null && entity.Medications.Any())
            {
                tvp.Columns.Add(new DataColumn("ID", typeof(int)));
                foreach (int id in entity.Medications.Select(m => m.ID))
                    tvp.Rows.Add(id);
                IDbDataParameter tabelValueParameter = this.DataContext.CreateParameter("@medicationList", tvp, ParameterDirection.Input);
                paramList.Add(tabelValueParameter);
            }
            await this.DataContext.ExecuteSPNonQueryAsync(commandName, paramList);
        }

        /// <summary>
        /// sets the medications to the user
        /// </summary>
        /// <param name="users">collection of users</param>
        /// <returns></returns>
        private async Task SetUserMedications(IEnumerable<User> users)
        {
            string commandName = "[dbo].[getUserMedications]";
            foreach (User user in users)
            {
                List<IDbDataParameter> paramList = new List<IDbDataParameter>();
                AddParameter(paramList, "@userID", user.ID, ParameterDirection.Input, DbType.Int32);
                user.Medications = await this.DataContext.ExecuteSPReaderAsync<ICollection<Medication>>(commandName, paramList, MapMedicationsFromReader);
            }
        }

        /// <summary>
        /// maps the user object from the reader
        /// </summary>
        /// <param name="reader">data reader</param>
        /// <returns>user object</returns>
        private User MapUserFromReader(DbDataReader reader)
        {
            User user = new User
            {
                Medications = new List<Medication>()
            };
            
            while (reader.Read())
            {
                user.ID = reader.GetValue<int>("ID");
                user.Name = reader.GetValue<string>("Name");
                user.EmailAddress = reader.GetValue<string>("EmailAddress");
                user.Province = new Province { ID = reader.GetValue<int>("ProvinceID") };
                user.State = (UserState)reader.GetValue<int>("State");
                user.DateCreated = reader.GetValue<DateTime>("DateCreated");
                user.LastUpdated = reader.GetValue<DateTime>("LastUpdated");
                user.Deleted = reader.GetValue<bool>("Deleted");
            }

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    Medication medication = new Medication
                    {
                        ID = reader.GetValue<int>("ID"),
                        Name = reader.GetValue<string>("Name")
                    };
                    user.Medications.Add(medication);
                }
            }

            return user;
        }

        /// <summary>
        /// maps a collection of user objects from reader
        /// </summary>
        /// <param name="reader">data reader</param>
        /// <returns>collection of users</returns>
        private IEnumerable<User> MapUsersFromReader(DbDataReader reader)
        {
            ICollection<User> users = new List<User>();
            while (reader.Read())
            {
                User user = new User
                {
                    Medications = new List<Medication>(),
                    ID = reader.GetValue<int>("ID"),
                    Name = reader.GetValue<string>("Name"),
                    EmailAddress = reader.GetValue<string>("EmailAddress"),
                    Province = new Province { ID = reader.GetValue<int>("ProvinceID") },
                    State = (UserState)reader.GetValue<int>("State"),
                    DateCreated = reader.GetValue<DateTime>("DateCreated"),
                    LastUpdated = reader.GetValue<DateTime>("LastUpdated"),
                    Deleted = reader.GetValue<bool>("Deleted")
                };
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// maps medication object from reader
        /// </summary>
        /// <param name="reader">data reader</param>
        /// <returns>collection of medications</returns>
        private ICollection<Medication> MapMedicationsFromReader(DbDataReader reader)
        {
            ICollection<Medication> medications = new List<Medication>();
            while (reader.Read())
            {
                Medication medication = new Medication
                {
                    ID = reader.GetValue<int>("ID"),
                    Name = reader.GetValue<string>("Name")
                };
                medications.Add(medication);
            }

            return medications;
        }
    }
}
