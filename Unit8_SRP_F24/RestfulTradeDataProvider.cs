using System.Text.Json;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class RestfulTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string url;
    private readonly ILogger logger;

    public RestfulTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    // Keep the current interface while making the network call asynchronous.
    public IEnumerable<string> ReadTradeData()
    {
        return Task.Run(ReadTradeDataAsync).GetAwaiter().GetResult();
    }

    private async Task<List<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from Restful API: " + url);

        using HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogMessage("Could not load trades from REST API. HTTP status: " + response.StatusCode);
            response.EnsureSuccessStatusCode();
        }

        string json = await response.Content.ReadAsStringAsync();
        List<string>? tradeData = JsonSerializer.Deserialize<List<string>>(json);

        if (tradeData == null)
        {
            throw new JsonException("The REST API returned a null trade list.");
        }

        logger.LogMessage("Received " + tradeData.Count + " trades from REST API.");
        return tradeData;
    }
}
