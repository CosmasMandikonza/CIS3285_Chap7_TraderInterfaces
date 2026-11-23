using System.Xml.Linq;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class ConsoleLogger : ILogger
{
    private static readonly object fileLock = new object();
    private readonly string logFilePath;

    // By default, save the XML log in the application's working directory.
    // A custom path also makes this class easy to test without touching real logs.
    public ConsoleLogger(string? logFilePath = null)
    {
        this.logFilePath = logFilePath ?? "trades.xml";
    }

    public void LogMessage(string message, params object[] args)
    {
        // Keep the existing console behavior while saving the same message to XML.
        string formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
        Console.WriteLine(formattedMessage);

        string type = "INFO";
        if (formattedMessage.StartsWith("WARN:", StringComparison.OrdinalIgnoreCase) ||
            formattedMessage.StartsWith("WARNING:", StringComparison.OrdinalIgnoreCase))
        {
            type = "WARN";
        }
        else if (formattedMessage.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
        {
            type = "ERROR";
        }

        lock (fileLock)
        {
            string fullPath = Path.GetFullPath(logFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            // One <logs> root keeps the file valid XML even after many messages.
            XDocument document = File.Exists(fullPath)
                ? XDocument.Load(fullPath)
                : new XDocument(new XElement("logs"));

            document.Root!.Add(
                new XElement("log",
                    new XElement("type", type),
                    new XElement("message", formattedMessage)));

            document.Save(fullPath);
        }
    }
}
