# klickinterview

## Online Dictionary

A web app based to find definition of a word using HTML and ES6+.

### Prerequisites

1. NodeJS

### Steps to run the application

1. Run - npm install in OnlineDictionary directory.
2. Run  - npm run start in OnlineDictionary directory.


## Simple User Database

A class library providing an API to interact with simple user database using .NET Core.

### Assumptions

1. A user must have a name.
2. A user must have an email address.
3. A user must have a province.
4. A user can be taking multiple medicines at the same time. (Many-to-many relationship between user and medication).
5. Deleting a medication for a user should not delete the record from the database for medical history purpose.

### Prerequisites

1. MS SQL Server
2. .NET Core 3.1
3. NuGet Package Manager

### Steps to run the application

1. Run the script SchemaScripts.sql in directory SimpleUserDatabase to create the database and schema.
2. Run the script SeedDataScripts.sql in directory SimpleUserDatabase to seed reference data.
        --OR--
1. Restore backup of my dev database SimpleUserDatabase.bak in directory SimpleUserDatabase.

3. Update the connection string 'KlickDatabaseConnectionString' in appsettings.json in Klick.SimpleUserDatabase.ConsoleClient.
4. Run the test console client Klick.SimpleUserDatabase.ConsoleClient.