using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class URLTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string url;
    private readonly ILogger logger;

    public URLTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    // ITradeDataProvider has a synchronous method, so run the HTTP work in a task.
    public IEnumerable<string> ReadTradeData()
    {
        return Task.Run(ReadTradeDataAsync).GetAwaiter().GetResult();
    }

    private async Task<List<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from URL: " + url);

        using HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogMessage("Could not load trades from URL. HTTP status: " + response.StatusCode);
            response.EnsureSuccessStatusCode();
        }

        List<string> tradeData = new List<string>();
        using Stream stream = await response.Content.ReadAsStreamAsync();
        using StreamReader reader = new StreamReader(stream);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            tradeData.Add(line);
        }

        return tradeData;
    }
}
