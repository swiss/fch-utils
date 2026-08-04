using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Swiss.FCh.Utils.Converter;

/// <summary>
/// A JSON converter for <see cref="string"/> values that removes unsupported control characters during deserialization.
/// </summary>
/// <remarks>
/// During deserialization, all control characters are removed except tab (TAB), line feed (LF), and carriage return (CR).
/// Printable ASCII characters, whitespace, and valid Unicode characters are preserved.
/// <para>
/// Serialization behavior is identical to the default <see cref="JsonConverter{T}"/> for <see cref="string"/>.
/// </para>
/// </remarks>
public partial class SanitizeStringJsonConverter : JsonConverter<string>
{
    private const string AllowedCharsRegex = @"[^\u0009\u000A\u000D\u0020-\u007E\u0080-\uffff]";
    private static readonly JsonConverter<string> _fallbackConverter = (JsonConverter<string>)JsonSerializerOptions.Default.GetConverter(typeof(string));

    /// <summary>
    /// Determines whether this converter can convert the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="typeToConvert"/> is <see cref="string"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    /// <summary>
    /// Reads a JSON string value and removes unsupported control characters.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>
    /// The sanitized string value, or <see langword="null"/> if the JSON value is <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// Empty and whitespace-only strings are returned unchanged.
    /// </remarks>
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

    /// <summary>
    /// Writes the specified string value using the default JSON string converter.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The string value to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        _fallbackConverter.Write(writer, value, options);
    }

    [GeneratedRegex(AllowedCharsRegex)]
    private static partial Regex InputSanitizeRegex();
}
