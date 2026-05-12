using System.Text.Json;
using Swiss.FCh.Utils.Converter;

namespace Swiss.FCh.Utils.Tests.Converter;

[TestFixture]
internal sealed class SanitizeStringJsonConverterTests
{
    [Test]
    public void CanConvert_ForStringType_ShouldReturnTrue()
    {
        var result = new SanitizeStringJsonConverter().CanConvert(typeof(string));

        Assert.That(result, Is.True);
    }

    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Test", "Test")]
    [TestCase("Overkill", "Overkill")]
    public void Read_WithInput_ShouldReturnSanitizedString(string? value, string? expected)
    {
        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(value);
        var reader = new Utf8JsonReader(jsonBytes.AsSpan());
        reader.Read();

        var converter = new SanitizeStringJsonConverter();
        var result = converter.Read(ref reader, typeof(string), new JsonSerializerOptions());

        Assert.That(result, Is.EqualTo(expected));
    }
}
