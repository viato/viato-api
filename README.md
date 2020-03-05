# viato-api

### Local-Development
- Install [PostgreSQL 12.2](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
- Open pgAdmin and login to localhost server
- Create Database named "viato"
- Create DB user (with all permissions) User Id = dev, Password = ZcRcAf3vzFeZtxHG
- In Visual Studio Open PM Console and type: update-database

#### Local-Development: Adding migration
After adding/modifying any database entity please run following command from PM Console:
- add-migration [your_migration_name]
- update-database

### Local-Testing
You can find postman collection at: `/tools/Viato.postman_collection.json` .

Also you can user swagger which is enabled by default for dev environment.

### Coding Standards

viato-api is mainly using [DotNet Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
Except few things, like we name private fields with underscore like: `_cloudTableClinet` and we don't use `this.` qualifier. (This behavior is controlled by analyzers, and in case of not matching code, they will raise build warning. )
We have adopted main [StyleCop Rules](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/DOCUMENTATION.md) and we are using [Style Cop DotNet analyzer](https://github.com/DotNetAnalyzers/StyleCopAnalyzers), that will react if something doesn't match to our standards.
