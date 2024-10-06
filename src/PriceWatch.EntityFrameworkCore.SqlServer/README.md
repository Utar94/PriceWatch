# PriceWatch.EntityFrameworkCore.SqlServer

TODO

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to provide a migration name!

```sh
dotnet ef migrations add <YOUR_MIGRATION_NAME> --context PriceWatchContext --project src/PriceWatch.EntityFrameworkCore.SqlServer --startup-project src/PriceWatch.Worker
```

### Remove a migration

To remove the latest unapplied migration, execute the following command.

```sh
dotnet ef migrations remove --context PriceWatchContext --project src/PriceWatch.EntityFrameworkCore.SqlServer --startup-project src/PriceWatch.Worker
```

### Generate a script

To generate a script, execute the following command. Do not forget to provide a source migration name!

```sh
dotnet ef migrations script <SOURCE_MIGRATION> --context PriceWatchContext --project src/PriceWatch.EntityFrameworkCore.SqlServer --startup-project src/PriceWatch.Worker
```