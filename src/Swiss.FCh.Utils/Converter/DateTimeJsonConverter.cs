using System.Text.Json;
using System.Text.Json.Serialization;

namespace Swiss.FCh.Utils.Converter;

// https://github.com/dotnet/runtime/issues/1566#issuecomment-745201271
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime().ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStringValue(value.ToUniversalTime());
    }
}
