using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class SimpleTradeValidator : ITradeValidator
{
    private readonly ILogger logger;

    public SimpleTradeValidator(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Validate the three fields of one currency trade: currency pair, amount, and price.
    /// </summary>
    public bool ValidateTradeData(string[] fields, int currentLine)
    {
        if (fields.Length != 3)
        {
            logger.LogMessage("WARN: Line {0} malformed. Only {1} field(s) found.", currentLine, fields.Length);
            return false;
        }

        // A currency pair contains two three-letter currency codes (for example, GBPUSD).
        if (string.IsNullOrWhiteSpace(fields[0]) ||
            fields[0].Length != 6 ||
            !fields[0].All(char.IsLetter))
        {
            logger.LogMessage("WARN: Trade currencies on line {0} malformed: '{1}'", currentLine, fields[0]);
            return false;
        }

        if (!int.TryParse(fields[1], out int tradeAmount))
        {
            logger.LogMessage("WARN: Trade amount on line {0} not a valid integer: '{1}'", currentLine, fields[1]);
            return false;
        }

        // User Story 904: reject potentially dangerous amounts outside these inclusive limits.
        if (tradeAmount < 1000 || tradeAmount > 1000000)
        {
            logger.LogMessage("WARN: Trade amount on line {0} must be between 1000 and 1000000: '{1}'",
                currentLine, tradeAmount);
            return false;
        }

        if (!decimal.TryParse(fields[2], out decimal tradePrice))
        {
            logger.LogMessage("WARN: Trade price on line {0} not a valid decimal: '{1}'", currentLine, fields[2]);
            return false;
        }

        if (tradePrice <= 0)
        {
            logger.LogMessage("WARN: Trade price on line {0} must be positive: '{1}'", currentLine, tradePrice);
            return false;
        }

        return true;
    }
}
