using System.Globalization;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Swiss.FCh.Utils.Logging;

/// <summary>
/// Custom Serilog formatter for formatting log messages to JSON according to the naming conventions of the FOITT.
/// The implementation is leaned on Serilog's own formatters
/// See: https://github.com/serilog/serilog-formatting-compact/blob/dev/src/Serilog.Formatting.Compact/RenderedCompactJsonFormatter.cs
/// </summary>
public sealed class SerilogFormatter : ITextFormatter
{
    private static readonly JsonValueFormatter _jsonValueFormatter = new();

    private static readonly Dictionary<LogEventLevel, string> _logLevelMapping = new()
    {
        { LogEventLevel.Verbose, "TRACE" },
        { LogEventLevel.Debug, "DEBUG" },
        { LogEventLevel.Information, "INFO" },
        { LogEventLevel.Warning, "WARN" },
        { LogEventLevel.Error, "ERROR" },
        { LogEventLevel.Fatal, "ERROR" }
    };

    /// <summary>
    /// Here, we write a string to the output that needs to be valid JSON, including all the properties, that are
    /// required alongside the message itself.
    ///
    /// Currently, the JSON is populated with the following data:
    /// - @timestamp
    /// - level (TRACE; DEBUG; INFO; WARN; ERROR
    /// - message
    /// - message_template
    /// - all the properties rendered into the template
    /// - exception (if provided)
    /// </summary>
    /// <remarks>
    /// Essentially, the formatter writes the JSON syntax manually. While the code may not be readable very well, this is the most
    /// efficient way regarding performance and memory allocations.
    /// When changing the code, it must be ensured, that the formatter always returns valid JSON syntax.
    /// </remarks>
    /// <param name="logEvent">The log event handed to this formatter by Serilog</param>
    /// <param name="output">The TextWriter that has to be used to write the log message to</param>
    public void Format(LogEvent logEvent, TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(output);

        try
        {
            output.WriteLine();

            output.Write("{ ");

            output.Write("\"@timestamp\": ");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.Timestamp.ToString("o"), output);

            output.Write(", \"level\": ");
            JsonValueFormatter.WriteQuotedJsonString(_logLevelMapping[logEvent.Level], output);

            output.Write(", \"message\": ");
            var message = logEvent.MessageTemplate.Render(logEvent.Properties, CultureInfo.InvariantCulture);
            JsonValueFormatter.WriteQuotedJsonString(message, output);

            output.Write(", \"message_template\": ");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);

            if (logEvent.Properties.TryGetValue("SourceContext", out var c) && c is ScalarValue { Value: string sourceName })
            {
                output.Write(", \"logger\": ");
                JsonValueFormatter.WriteQuotedJsonString(sourceName, output);
            }

            if (logEvent.Properties.TryGetValue("ThreadName", out var threadNameProp) && threadNameProp is ScalarValue { Value: string threadName })
            {
                output.Write(", \"thread_name\": ");
                JsonValueFormatter.WriteQuotedJsonString(threadName, output);
            }

            foreach (var prop in logEvent.Properties)
            {
                // Avoid duplicating keys already written
                if (prop.Key is "SourceContext" or "ThreadName" or "ThreadId")
                {
                    continue;
                }

                output.Write(",");

                if (prop.Key == "CorrelationId") //rename the CorrelationId property
                {
                    output.Write("\"correlation_id\"");
                }
                else //add anything else as is
                {
                    JsonValueFormatter.WriteQuotedJsonString(prop.Key, output);
                }

                output.Write(": ");

                //the property value can be anything (even a list of objects), that is why we need the JsonValueFormatter to format it as valid JSON
                _jsonValueFormatter.Format(prop.Value, output);
            }

            if (logEvent.Exception != null)
            {
                output.Write(", \"exception\": ");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            output.Write(" }");
        }
        catch (Exception e)
        {
            output.WriteLine($"{{\"message\": \"Error in {typeof(SerilogFormatter).FullName}: " + e.Message + "\"}}");
        }
    }
}
