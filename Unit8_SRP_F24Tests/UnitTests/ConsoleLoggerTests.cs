using System.Xml.Linq;
using Unit8_SRP_F24;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass]
public class ConsoleLoggerTests
{
    [TestMethod]
    public void LogMessage_WritesInfoWarningsAndErrorsToXml()
    {
        // Arrange: use a temporary file so the test does not need a real log file.
        string logFile = Path.Combine(Path.GetTempPath(), "unit9-trades-" + Guid.NewGuid() + ".xml");
        var logger = new ConsoleLogger(logFile);

        try
        {
            // Act
            logger.LogMessage("4 trades processed");
            logger.LogMessage("WARN: Line {0} malformed", 2);
            logger.LogMessage("ERROR: Could not save trades");

            // Assert
            XDocument document = XDocument.Load(logFile);
            var entries = document.Root!.Elements("log").ToList();
            Assert.AreEqual("logs", document.Root.Name.LocalName);
            Assert.AreEqual(3, entries.Count);
            Assert.AreEqual("INFO", entries[0].Element("type")?.Value);
            Assert.AreEqual("4 trades processed", entries[0].Element("message")?.Value);
            Assert.AreEqual("WARN", entries[1].Element("type")?.Value);
            Assert.AreEqual("WARN: Line 2 malformed", entries[1].Element("message")?.Value);
            Assert.AreEqual("ERROR", entries[2].Element("type")?.Value);
            Assert.AreEqual("ERROR: Could not save trades", entries[2].Element("message")?.Value);
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }

    [TestMethod]
    public void LogMessage_EscapesSpecialXmlCharacters()
    {
        // Arrange
        string logFile = Path.Combine(Path.GetTempPath(), "unit9-trades-" + Guid.NewGuid() + ".xml");
        var logger = new ConsoleLogger(logFile);

        try
        {
            // Act
            logger.LogMessage("WARN: Unexpected <trade> & value");

            // Assert: XML must still parse, and the original message must survive.
            XDocument document = XDocument.Load(logFile);
            Assert.AreEqual(
                "WARN: Unexpected <trade> & value",
                document.Root!.Element("log")!.Element("message")?.Value);
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}
