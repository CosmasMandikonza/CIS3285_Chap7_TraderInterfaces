# Unit8_SRP_F24

## Requirements

- .NET 10 SDK
- Visual Studio Code with the C# Dev Kit extension
- A configured SQL Server database for trade storage

Both projects target `net10.0`. This targets the .NET 10 release without pinning
the project to a particular .NET 10 patch version.

## Open and run in Visual Studio Code

Open this repository folder in VS Code. In the integrated terminal, build the
solution and run the sample application with:

```powershell
dotnet build Unit8_SRP_F24.sln
dotnet run --project Unit8_SRP_F24/Unit8_SRP_F24.csproj
```

Run the test project with:

```powershell
dotnet test Unit8_SRP_F24.sln
```

The sample writes trades to a LocalDB database using the connection string in
`TradeProcessor.cs`. The integration test also queries Azure SQL using the
connection string in `TradeProcessorTests.cs`. Configure both for databases
available in your environment before running the application or tests; the test
inserts trade records.

## Unit 9 Learning Activity: Read from HTTP

This fork implements **Part 1 (User Story 902)** and **Part 2 (User Story 903)**.

- `URLTradeDataProvider` downloads a remote text file using `HttpClient` and returns one trade string per line.
- `RestfulTradeDataProvider` requests JSON from the REST API, reads the response as a string, and converts it to `List<string>` with `JsonSerializer`.
- Both providers keep the existing `ITradeDataProvider.ReadTradeData()` interface and run their HTTP work in asynchronous tasks. `TradeProcessor` did not need to change.
- `Program.cs` uses the REST provider by default; pass `--url` to select the remote text-file provider.

Run the learning activity's HTTP tests:

```powershell
dotnet test Unit8_SRP_F24Tests/Unit8_SRP_F24Tests.csproj --filter "FullyQualifiedName~URLTradeDataProviderTests|FullyQualifiedName~RestfulTradeDataProviderTests"
```

Run with either source (the application also requires the SQL Server storage configuration described above):

```powershell
dotnet run --project Unit8_SRP_F24/Unit8_SRP_F24.csproj
dotnet run --project Unit8_SRP_F24/Unit8_SRP_F24.csproj -- --url
```

The URL tests cover the existing **1-trade** and **5-trade** URLs and the **4-trade Fall 2024** URL from the assignment. The REST test expects **3 trades** from the instructor's HTTPS API.

**Note:** The full test suite includes database-dependent tests requiring SQL Server. The dedicated GitHub Actions workflow focuses on the learning activity's HTTP provider tests and does not require a database.
