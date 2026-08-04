using System.Text.Json;
using System.Text.Json.Serialization;

namespace Swiss.FCh.Utils.Converter;

/// <summary>
/// This converter (for System.Text.Json) reads and writes dates as UTC.
/// See: https://github.com/dotnet/runtime/issues/1566#issuecomment-745201271
/// </summary>
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// Reads a value as <see cref="System.DateTime"/> with <see cref="System.DateTimeKind.Utc"/>.
    /// </summary>
    /// <param name="reader"><see cref="System.Text.Json.Utf8JsonReader"/> to read the JSON.</param>
    /// <param name="typeToConvert"><see cref="System.Type"/> to convert into (not used as <see cref="System.DateTime"/> is fixed in this case).</param>
    /// <param name="options"><see cref="System.Text.Json.JsonSerializerOptions"/>, not used in this case.</param>
    /// <returns><see cref="System.DateTime"/> with <see cref="System.DateTimeKind.Utc"/></returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime().ToUniversalTime();
    }

    /// <summary>
    /// Writes a <see cref="System.DateTime"/> value converted to UTC as JSON.
    /// </summary>
    /// <param name="writer"><see cref="System.Text.Json.Utf8JsonWriter"/> to write the JSON</param>
    /// <param name="value"><see cref="System.DateTime"/> value that should be written as JSON</param>
    /// <param name="options"><see cref="System.Text.Json.JsonSerializerOptions"/>, not used in this case</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStringValue(value.ToUniversalTime());
    }
}
