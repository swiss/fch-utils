using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Swiss.FCh.Utils.Converter;

public partial class SanitizeStringJsonConverter : JsonConverter<string>
{
    private const string AllowedCharsRegex = @"[^\u0009\u000A\u000D\u0020-\u007E\u0080-\uffff]";
    private static readonly JsonConverter<string> _fallbackConverter = (JsonConverter<string>)JsonSerializerOptions.Default.GetConverter(typeof(string));

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (reader.TokenType != JsonTokenType.String)
        {
            return value;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        // Remove control-characters from our strings. We don't want to store them
        // We accept:
        //      - TAB
        //      - LF
        //      - CR
        //      - Space
        //      - ~
        //      - All other Unicode except DEL
        value = InputSanitizeRegex().Replace(value, string.Empty);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        _fallbackConverter.Write(writer, value, options);
    }

    [GeneratedRegex(AllowedCharsRegex)]
    private static partial Regex InputSanitizeRegex();
}
