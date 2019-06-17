# Images seeder
There's a .NET Core 2.0 console project to seed the images needed by the API services' consumers.

## Customizing the Project
The project's code is located in [`SeedImages/SmarthHotel.Services.Seed.Images`](../../Source/Backend/src/SmartHotel.Services.Seed.Images), and requires one minor customization.
There are some env based appsettings file with the following keys:
```json
{
  "HotelsConnectionString": "", // the SQL Server connection string to the Hotels SQL Server database
  "StorageAccountName": "", // the Azure Storage where the images will be loaded
  "StorageConnectionString": "", // the Azure Storage connection string
}
```
You'll simply need to give the proper value from your Storages and SQL Server for each env based appsettings file you need.

---

## Running the project

To run it from command line:
```bash
cd Source/Backend/src/SmartHotel.Services.Seed.Images
dotnet build 
dotnet run
```

To run it in Visual Studio:
1. Double click in `SmartHotel.Services.Seed.Images.csproj` file.
2. Click in the `Run` button.

    ![Run in Visual Studio](../media/run-seeder-vs.png)