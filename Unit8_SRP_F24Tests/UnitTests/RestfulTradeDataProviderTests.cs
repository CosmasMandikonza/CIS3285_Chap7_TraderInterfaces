using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class RestfulTradeDataProviderTests
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
    public void TestSize3()
    {
        // Arrange: use the HTTPS endpoint listed in the Unit 9 assignment.
        ILogger logger = new ConsoleLogger();
        string restfulURL = "https://unit9trader.azurewebsites.net/api/TradeData";
        ITradeDataProvider provider = new RestfulTradeDataProvider(restfulURL, logger);

        // Act
        IEnumerable<string> trades = provider.ReadTradeData();

        // Assert
        Assert.AreEqual(3, CountStrings(trades));
    }
}
