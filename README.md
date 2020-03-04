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
You can find postman collection at: `/tools/Viato.postman_collection.json`
