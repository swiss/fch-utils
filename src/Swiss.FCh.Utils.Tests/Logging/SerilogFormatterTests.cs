using Swiss.FCh.Utils.Logging;
using Serilog.Events;
using Serilog.Parsing;

namespace Swiss.FCh.Utils.Tests.Logging;

[TestFixture]
internal sealed class SerilogFormatterTests
{
    private readonly SerilogFormatter _formatter = new();

    [Test]
    public void Format_WithValidInput_RendersValidJsonMessage()
    {
        var now = DateTimeOffset.Now;

        var logEvent = new LogEvent(
            now,
            LogEventLevel.Fatal,
            new ArgumentException("test \"exception\""),
            new MessageTemplate([new TextToken("test error \"{message}\"")]) , [new LogEventProperty("message", new ScalarValue("my message"))]);

        var output = new StringWriter();

        _formatter.Format(logEvent, output);

        var renderedMessage = output.ToString();

        Assert.That(renderedMessage, Is.EqualTo("\r\n{ \"@timestamp\": \"" + now.ToString("o") + "\", \"level\": \"ERROR\", \"message\": \"test error \\\"{message}\\\"\", \"message_template\": \"test error \\\"{message}\\\"\",\"message\": \"my message\", \"exception\": \"System.ArgumentException: test \\\"exception\\\"\" }"));

        output.Dispose();
    }
}
