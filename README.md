# viato-api

[![Build status](https://ci.appveyor.com/api/projects/status/oivxuthaj2ttnss0/branch/master?svg=true)](https://ci.appveyor.com/project/arkoc/viato-api/branch/master)

## Local-Development
- Install [PostgreSQL 12.2](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
- Open pgAdmin and login to localhost server
- Create Database named "viato"
- Create DB user (with all permissions) User Id = dev, Password = ZcRcAf3vzFeZtxHG
- In Visual Studio Open PM Console and type: update-database
- Make sure you have [Azure storage emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) installed and running.

#### Local-Development: Adding migration
After adding/modifying any database entity please run following command from PM Console:
- add-migration [your_migration_name]
- update-database

#### Local-Testing
You can find postman collection at: `/tools/Viato.postman_collection.json`.

Also you can user swagger which is enabled by default for dev environment and available at: `/swagger`.

## Coding Standards

viato-api is mainly using [DotNet Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)

Except few things, like we name private fields with underscore like: `_dbContext` and we don't use `this.` qualifier. (This behavior is controlled by analyzers, and in case of not matching code, they will raise build warning. )

We have adopted main [StyleCop Rules](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/DOCUMENTATION.md) and we are using [Style Cop DotNet analyzer](https://github.com/DotNetAnalyzers/StyleCopAnalyzers), that will react if something doesn't match to our standards.


## Coding Conventions

#### Error Handling

For any model realted error we use default `ModelState` of `Controller`. For any custom error not related to model we use customer http status codes, for example:

```
  public sealed class AppHttpErrors
  {
      public const int TorPipelineNotFound = 441;
      public const int TorPipelineIsNotAcitve = 442;
      public const int TorOrganizationNotVerified = 443;
      ...
      public const int User2FAAlreadyEnabled = 451;
  }
```

All custom http errors should be defined in [AppHttpErrors.cs](https://github.com/viato/viato-api/blob/master/src/Viato.Api/AppHttpErrors.cs) file.
