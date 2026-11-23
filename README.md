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

The sample application uses `AdoNetTradeStorage` to store trades in SQL Server.
Configure that connection for your own environment before running the console
application. The current automated tests, including the `TradeProcessorTests`,
use test data and stub implementations; they do not need a SQL Server connection.

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

## Unit 9 Assignment: Trade validation, XML logging, and tests

This fork also completes **Parts 3–5 of the Unit 9 Assignment**.

- `SimpleTradeValidator` allows trade amounts from **1,000 to 1,000,000**, inclusive, and rejects malformed currency codes, invalid amounts, and non-positive prices.
- `ConsoleLogger` prints to the console and writes a valid `trades.xml` file. It keeps entries under a `<logs>` root, with `<log><type>...</type><message>...</message></log>` for each message.
- `SimpleTradeValidatorTests` includes `EmptyCurrencyString_ReturnsFalse`, and `ConsoleLoggerTests` verifies XML log entries and safe escaping of special characters.
- Each requested assignment part was committed separately.
- The written **Unit 8 vs Unit 9 test comparison** and submission link are in [SUBMISSION.md](SUBMISSION.md).

Run all tests:

```powershell
dotnet test Unit8_SRP_F24.sln
```

The GitHub Actions **Unit 9 Full Test Suite** workflow runs all tests with .NET 10.
The runtime application still needs the configured SQL Server storage connection,
but the automated test suite does not.
