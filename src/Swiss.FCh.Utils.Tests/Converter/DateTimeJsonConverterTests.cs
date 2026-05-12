using System.Text;
using System.Text.Json;
using Swiss.FCh.Utils.Converter;

namespace Swiss.FCh.Utils.Tests.Converter;

[TestFixture]
internal sealed class DateTimeJsonConverterTests
{
    [Test]
    public void Read_ValidDateTimeInput_ShouldReturnsDateTimeInUtc()
    {
        var dateToTest = new DateTime(2023, 7, 11, 15, 30, 0, DateTimeKind.Local);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dateToTest);
        var reader = new Utf8JsonReader(bytes.AsSpan());
        reader.Read();

        var converter = new DateTimeJsonConverter();
        var result = converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());

        Assert.That(result, Is.EqualTo(dateToTest.ToUniversalTime()));
        Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void Write_DateTimeValue_ShouldWritesUtcString()
    {
        var dateToTest = new DateTime(2023, 7, 11, 15, 30, 0, DateTimeKind.Utc);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        var converter = new DateTimeJsonConverter();
        converter.Write(writer, dateToTest, new JsonSerializerOptions());

        writer.Flush();
        var jsonString = Encoding.UTF8.GetString(stream.ToArray());

        Assert.That(jsonString.Trim(), Is.EqualTo($"\"{dateToTest.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}\""));
    }
}
