using Swiss.FCh.Utils.Models;
using Swiss.FCh.Utils.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Swiss.FCh.Utils.Tests.Services;

[TestFixture]
internal sealed class HtmlNormalizerTests
{
    private readonly ILogger<HtmlNormalizer> _logger = NullLogger<HtmlNormalizer>.Instance;

    private HtmlNormalizer _htmlNormalizer = null!;

    [SetUp]
    public void SetUp()
    {
        _htmlNormalizer = new HtmlNormalizer(_logger);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Normalize_WithEmptyOrNullContent_ShouldReturnNull(string? content)
    {
        var result = _htmlNormalizer.Normalize(content);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void Normalize_WithNonHtml_ShouldReturnContent()
    {
        const string content = "test_content";
        var result = _htmlNormalizer.Normalize(content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public void Normalize_WithRemoveClasses_ShouldRemoveClasses()
    {
        var options = new HtmlNormalizerOptions { RemoveClasses = true };
        const string html = "<h1 class=\"tests\"></h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<h1></h1>"));
    }

    [Test]
    public void Normalize_WithRemoveStyles_ShouldRemoveStyles()
    {
        var options = new HtmlNormalizerOptions { RemoveStyles = true };
        const string html = "<h1 style=\"background:red;\"></h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<h1></h1>"));
    }

    [Test]
    public void Normalize_WithRemoveStyles_ShouldRemoveStylesButKeepBoldAsStrong()
    {
        var options = new HtmlNormalizerOptions { RemoveStyles = true };
        const string html = "<h1 style=\"background:red;font-weight:bold\">Test</h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<strong><h1>Test</h1></strong>"));
    }

    [Test]
    public void Normalize_WithRemoveStyles_ShouldRemoveStylesButKeepItalicAsEm()
    {
        var options = new HtmlNormalizerOptions { RemoveStyles = true };
        const string html = "<h1 style=\"background:red;font-style:italic\">Test</h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<em><h1>Test</h1></em>"));
    }

    [Test]
    public void Normalize_WithRemoveStyles_ShouldRemoveStylesButKeepUnderlineAsU()
    {
        var options = new HtmlNormalizerOptions { RemoveStyles = true };
        const string html = "<h1 style=\"background:red;text-decoration:underline\">Test</h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<u><h1>Test</h1></u>"));
    }

    [Test]
    public void Normalize_WithRemoveImages_ShouldRemoveImages()
    {
        var options = new HtmlNormalizerOptions { RemoveImages = true };
        const string html = "<div><img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAADUlEQVR42mP8/5+hHgAHgwJ/lB2jVgAAAABJRU5ErkJggg==\" alt=\"4x4 Black Image\" /></div>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<div></div>"));
    }

    [Test]
    public void Normalize_WithRemoveEmptyAnchorsAndNoLink_ShouldRemoveAnchor()
    {
        var options = new HtmlNormalizerOptions { RemoveEmptyAnchors = true };
        const string html = "<div><a /></div>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<div></div>"));
    }

    [Test]
    public void Normalize_WithRemoveEmptyAnchorsAndEmptyLink_ShouldRemoveAnchor()
    {
        var options = new HtmlNormalizerOptions { RemoveEmptyAnchors = true };
        const string html = "<h1><a href=\"\"/></h1>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<h1></h1>"));
    }

    [Test]
    public void Normalize_WithRemoveEmptyAnchorsAndEmptyLink_ShouldReplaceWithInnerContent()
    {
        var options = new HtmlNormalizerOptions { RemoveEmptyAnchors = true };
        const string html = "<div><a href=\"\">&nbsp;<span>Inner Content</span> with more text<a/></div>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<div>&nbsp;<span>Inner Content</span> with more text</div>"));
    }

    [Test]
    public void Normalize_WithReplaceItalicWithEmphasis_ShouldReplaceItalicWithEmphasis()
    {
        var options = new HtmlNormalizerOptions { ReplaceItalicWithEmphasis = true };
        const string html = "<div><i>before text<span>span_content</span><br>other text<div>div_content</div> last text</i></div>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<div><em>before text<span>span_content</span><br>other text<div>div_content</div> last text</em></div>"));
    }

    [Test]
    public void Normalize_WithReplaceBoldWithStrong_ShouldReplaceBoldWithStrong()
    {
        var options = new HtmlNormalizerOptions { ReplaceBoldWithStrong = true };
        const string html = "<div><b>before text<span>span_content</span><br>other text<div>div_content</div> last text</b></div>";

        var result = _htmlNormalizer.Normalize(html, options);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo("<div><strong>before text<span>span_content</span><br>other text<div>div_content</div> last text</strong></div>"));
    }

    [TestCase("TestData/HtmlNormalizer/15.1031_DE.html", "TestData/HtmlNormalizer/15.1031_DE_expected.html")]
    [TestCase("TestData/HtmlNormalizer/17.3244_IT.html", "TestData/HtmlNormalizer/17.3244_IT_expected.html")]
    [TestCase("TestData/HtmlNormalizer/18.3954_IT.html", "TestData/HtmlNormalizer/18.3954_IT_expected.html")]
    [TestCase("TestData/HtmlNormalizer/24.3702_DE.html", "TestData/HtmlNormalizer/24.3702_DE_expected.html")]
    [TestCase("TestData/HtmlNormalizer/27.9555_IT.html", "TestData/HtmlNormalizer/27.9555_IT_expected.html")]
    [TestCase("TestData/HtmlNormalizer/two.html", "TestData/HtmlNormalizer/two_expected.html")]
    [TestCase("TestData/HtmlNormalizer/sub_sup.html", "TestData/HtmlNormalizer/sub_sup_expected.html")]
    [TestCase("TestData/HtmlNormalizer/nested.html", "TestData/HtmlNormalizer/nested_expected.html")]
    [TestCase("TestData/HtmlNormalizer/strike-through.html", "TestData/HtmlNormalizer/strike-through_expected.html")]
    public void Normalize_ForFile_ShouldReturnExpected(string file, string expectedFile)
    {
        var content = File.ReadAllText(file);
        var expected = File.ReadAllText(expectedFile);

        var result = _htmlNormalizer.Normalize(content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(expected));
    }
}
