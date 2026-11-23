using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

class Program
{
    static void Main(string[] args)
    {
        const string tradeURL =
            "https://raw.githubusercontent.com/tgibbons-css/CIS3285_Unit9_F24/refs/heads/master/SingleResponsibilityPrinciple/trades.txt";
        const string restfulURL = "https://unit9trader.azurewebsites.net/api/TradeData";

        ILogger logger = new ConsoleLogger();
        ITradeValidator tradeValidator = new SimpleTradeValidator(logger);

        // Read from the REST API by default. Use --url to read the remote text file.
        // Both providers implement ITradeDataProvider, so the processor needs no changes.
        ITradeDataProvider tradeDataProvider = args.Contains("--url", StringComparer.OrdinalIgnoreCase)
            ? new URLTradeDataProvider(tradeURL, logger)
            : new RestfulTradeDataProvider(restfulURL, logger);

        ITradeMapper tradeMapper = new SimpleTradeMapper();
        ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
        ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

        var tradeProcessor = new TradeProcessor(tradeDataProvider, tradeParser, tradeStorage);
        tradeProcessor.ProcessTrades();
    }
}
