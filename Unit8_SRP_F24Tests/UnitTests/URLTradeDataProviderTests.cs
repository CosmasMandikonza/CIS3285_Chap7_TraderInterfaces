using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class URLTradeDataProviderTests
{
    private int CountStrings(IEnumerable<string> trades)
    {
        int count = 0;
        foreach (string trade in trades)
        {
            count++;
        }
        return count;
    }

    [TestMethod()]
    public void TestSize1()
    {
        // Arrange
        ILogger logger = new ConsoleLogger();
        string tradeURL =
            "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_1good.txt";
        ITradeDataProvider provider = new URLTradeDataProvider(tradeURL, logger);

        // Act
        IEnumerable<string> trades = provider.ReadTradeData();

        // Assert
        Assert.AreEqual(1, CountStrings(trades));
    }

    [TestMethod()]
    public void TestSize5()
    {
        // Arrange
        ILogger logger = new ConsoleLogger();
        string tradeURL =
            "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_5good.txt";
        ITradeDataProvider provider = new URLTradeDataProvider(tradeURL, logger);

        // Act
        IEnumerable<string> trades = provider.ReadTradeData();

        // Assert
        Assert.AreEqual(5, CountStrings(trades));
    }

    [TestMethod()]
    public void TestSize4Fall2024TradeFile()
    {
        // This is the trade URL specifically named in the Unit 9 learning activity.
        ILogger logger = new ConsoleLogger();
        string tradeURL =
            "https://raw.githubusercontent.com/tgibbons-css/CIS3285_Unit9_F24/refs/heads/master/SingleResponsibilityPrinciple/trades.txt";
        ITradeDataProvider provider = new URLTradeDataProvider(tradeURL, logger);

        IEnumerable<string> trades = provider.ReadTradeData();

        Assert.AreEqual(4, CountStrings(trades));
    }
}
