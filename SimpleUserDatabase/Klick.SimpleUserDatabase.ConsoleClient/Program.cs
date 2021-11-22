using Klick.SimpleUserDatabase.Models;
using Klick.SimpleUserDatabase.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Klick.SimpleUserDatabase.ConsoleClient
{
    class Program
    {
        static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddSingleton(typeof(IConfiguration), config);
            serviceProvider = serviceCollection.BuildServiceProvider();
            ExecuteTests();
            Console.ReadKey();
        }

        static void ExecuteTests()
        {
            // creates 40 different users
            for (int i = 0; i < 40; i++)
            {
                CreateNewUserTest();
            }

            UpdateUserTestChangeMedication();

            UpdateUserTestChangeEmail();

            DeactiveUserTest();

            SearchAllActiveUsersInOntarioTest();

            SearchAllActiveUsersInAlbertaTakingMigraneMedicineTest();

            SearchAllActiveUsersInOntarioTakingObesityMigraneMedTest();
        }

        static User CreateNewUserTest()
        {
            string name = $"Person{Guid.NewGuid()}";
            Random r = new Random();
            int provinceID = r.Next(1, 14);
            int medCount = r.Next(0, 6);
            User user = new User
            {
                Name = name,
                EmailAddress = $"{name}@provider.com",
                Province = new Province { ID = provinceID },
                State = UserState.Active,
                Medications = new List<Medication>()
            };

            for (int i = 0; i < medCount; i++)
            {
                Medication medication = new Medication { ID = i + 1 };
                user.Medications.Add(medication);
            }

            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            int userID = userServiceManager.CreateNewUser(user).Result;
            user.ID = userID;
            Console.WriteLine(userID);
            return user;
        }

        static void UpdateUserTestChangeMedication()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            User user = userServiceManager.GetExistingUser(9).Result;
            user.Medications.Remove(user.Medications.ElementAt(0));
            user.Medications.Add(new Medication { ID = 5 });
            userServiceManager.UpdateExistingUser(user).Wait();
        }

        static void UpdateUserTestChangeEmail()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            User user = userServiceManager.GetExistingUser(15).Result;
            user.EmailAddress = "person@newprovider.com";
            userServiceManager.UpdateExistingUser(user).Wait();
        }

        static void DeactiveUserTest()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            User user = userServiceManager.GetExistingUser(15).Result;
            user.State = UserState.Inactive;
            userServiceManager.UpdateExistingUser(user).Wait();
        }

        static void SearchAllActiveUsersInOntarioTest()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            // criteria
            User user = new User
            {
                State = UserState.Active,
                Province = new Province { ID = 9 }
            };

            var users = userServiceManager.Search(user).Result;
        }

        static void SearchAllActiveUsersInAlbertaTakingMigraneMedicineTest()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            // criteria
            User user = new User
            {
                State = UserState.Active,
                Province = new Province { ID = 1 },
                Medications = new List<Medication>
                {
                    new Medication { ID = 3 }
                }
            };

            var users = userServiceManager.Search(user).Result;
        }

        static void SearchAllActiveUsersInOntarioTakingObesityMigraneMedTest()
        {
            UserServiceManager userServiceManager = new UserServiceManager(serviceProvider);
            // criteria
            User user = new User
            {
                State = UserState.Active,
                Province = new Province { ID = 9 },
                Medications = new List<Medication>
                {
                    new Medication { ID = 3 }
                    ,new Medication { ID = 4 }
                }
            };

            var users = userServiceManager.Search(user).Result;
        }
    }
}
