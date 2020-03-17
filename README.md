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


## API Conventions

#### Entity Framework

- We don't use Eager or Lazy loading, you have to explicitly set includes for each query.

#### Error Handling

For any model realted error we use default `ModelState` of `Controller`. For any custom error/status not related to model we use customer http status codes, for example:

```
  public sealed class AppHttpStatusCodes
  {
      public const int TorTokenIdAlreadyCreated = 211;
      ...
      public const int TorPipelineNotFound = 441;
      public const int TorPipelineIsNotAcitve = 442;
      ...
      public const int User2FAAlreadyEnabled = 451;
  }
```

All custom http status codes should be defined in [AppHttpStatusCodes.cs](https://github.com/viato/viato-api/blob/master/src/Viato.Api/AppHttpStatusCodes.cs) file.

## Acknowledgements
viato-api is built using the following great open source projects:

* [ASP.NET Core](https://github.com/aspnet)
* [IdentityServer4](https://github.com/IdentityServer/IdentityServer4)
* [EntityFrameworkCore](https://github.com/dotnet/efcore)
* [Npgsql.EntityFrameworkCore.PostgreSQL](https://github.com/npgsql/efcore.pg)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [DnsClient](https://github.com/MichaCo/DnsClient.NET)
* [XUnit](https://github.com/xunit/xunit)

..by our [contributors](https://github.com/viato/viato-api/graphs/contributors)!

