# CIS 3285 - Unit 9 Assignment (Parts 3-5)

**GitHub fork:** https://github.com/CosmasMandikonza/CIS3285_Chap7_TraderInterfaces

## What I changed

- **Part 3 / User Story 904:** Updated `SimpleTradeValidator.ValidateTradeData()` to reject trade amounts below **1,000** or above **1,000,000**, while allowing both boundary values. I also fixed the existing tests for non-letter currency codes and negative trade prices.
- **Part 4 / User Story 905:** Updated `ConsoleLogger` to keep printing to the console and also write XML logs to `trades.xml`. The file contains a `<logs>` root with a `<log>` entry for each message, including `<type>` and `<message>`. INFO, WARN and ERROR entries are supported. I added tests for the XML output.
- **Part 5:** Added `EmptyCurrencyString_ReturnsFalse` to `SimpleTradeValidatorTests` to confirm that an empty first string (`""`) is rejected.

## Part 5 written answer: Comparing Unit 8 and Unit 9 tests

The Unit 8 test was more like an integration test. For example, `TestOneGoodTrade` created a `TradeProcessor`, read a trade file using an embedded stream, and checked the number of records in a SQL database before and after processing the trade. This meant the test relied on a database connection and several parts of the program working together. If the database was unavailable, the test could fail even when the trade-processing logic was correct.

The Unit 9 unit tests are more focused. In `SimpleTradeValidatorTests`, I pass a small array of strings, such as `{"AAABBB", "1000", "1.00"}`, straight into `ValidateTradeData()` and check whether the result is true or false. For my new test, the first field is empty (`""`), and the expected result is false. These tests do not require a database or SQL connection. Some also use a `StubLogger` so logging does not affect the test. There are also `TradeProcessorTests` that use simple replacement objects for the data provider, parser, and storage, instead of using the real database.

I think the Unit 9 tests are better **as unit tests** because they check one behavior at a time, are easier to run repeatedly, and make it easier to find where a problem happened. This is helpful for test-driven development because I can write a failing test, fix the specific method, and check it again. The Unit 8 test is still useful for checking whether the complete system works with a database, but it is less isolated. The HTTP tests in Unit 9 also still depend on live web services, so not every test in the new project is completely independent.

## Verification

Run the tests with:

```powershell
dotnet test Unit8_SRP_F24.sln
```

GitHub Actions confirmed **34 tests passed, 0 failed** in the full .NET 10 suite on November 23, 2026:

https://github.com/CosmasMandikonza/CIS3285_Chap7_TraderInterfaces/actions/runs/45232267454

The project was changed in separate commits for Parts 3, 4, and 5, as requested.
