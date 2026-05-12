using Swiss.FCh.Utils.Services;

namespace Swiss.FCh.Utils.Tests.Services;

[TestFixture]
internal sealed class StyleExtensionsTests
{
    [TestCase("", false)]
    [TestCase("font:underline", false)]
    [TestCase("text-decoration:underline", true)]
    [TestCase("text-decoration: underline", true)]
    [TestCase("   text-decoration:    underline    ", true)]
    [TestCase("text-decoration: underline;", true)]
    [TestCase("TEXT-decoration: UnderLINE;", true)]
    [TestCase("text-decoration: test underline test;", true)]
    [TestCase("font-style: italic; text-decoration: test underline test;", true)]
    public void HasUnderline_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasUnderline();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }

    [TestCase("", false)]
    [TestCase("font:line-through", false)]
    [TestCase("text-decoration:line-through", true)]
    [TestCase("text-decoration: line-through", true)]
    [TestCase("   text-decoration:    line-through    ", true)]
    [TestCase("text-decoration: line-through;", true)]
    [TestCase("text-decoration: test line-through test;", true)]
    [TestCase("vertical-align: sub; text-decoration: test line-through test;", true)]
    public void HasStrikeThrough_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasStrikeThrough();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }

    [TestCase("", false)]
    [TestCase("font--style:italic", false)]
    [TestCase("font-style:italic", true)]
    [TestCase("font-style: italic", true)]
    [TestCase("   font-style:    italic    ", true)]
    [TestCase("font-style: italic;", true)]
    [TestCase("FONT-stylE: ItaLiC;", true)]
    [TestCase("font-style: test italic test", true)]
    [TestCase("vertical-align: sub; font-style: test italic test", true)]
    public void HasItalic_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasItalic();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }

    [TestCase("", false)]
    [TestCase("font-weight:thin", false)]
    [TestCase("random:string;", false)]
    [TestCase("font-weight:bold", true)]
    [TestCase("font-weight: bold", true)]
    [TestCase("   font-weight:    bold    ", true)]
    [TestCase("   font-weight:    bold    ;", true)]
    [TestCase("font-weight: bold;", true)]
    [TestCase("FONT-weight: BoLD;", true)]
    [TestCase("font-weight: test bold test", true)]
    [TestCase("vertical-align: super; font-weight: test bold test; vertical-align: sub; ", true)]
    public void HasBold_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasBold();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }

    [TestCase("", false)]
    [TestCase("vertical-align:super", false)]
    [TestCase("random:string;", false)]
    [TestCase("vertical-align:sub", true)]
    [TestCase("vertical-align: sub", true)]
    [TestCase("   vertical-align:    sub    ", true)]
    [TestCase("   vertical-align:    sub    ;", true)]
    [TestCase("vertical-align: sub;", true)]
    [TestCase("vertical-AliGn: SUb;", true)]
    [TestCase("vertical-align: test sub test", true)]
    [TestCase("font:line-through; vertical-align: test sub test; vertical-align: super;", true)]
    public void HasSubscript_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasSubscript();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }

    [TestCase("", false)]
    [TestCase("vertical-align:sub", false)]
    [TestCase("random:string;", false)]
    [TestCase("vertical-align:super", true)]
    [TestCase("vertical-align: super", true)]
    [TestCase("   vertical-align:    super    ", true)]
    [TestCase("   vertical-align:    super    ;", true)]
    [TestCase("vertical-align: super;", true)]
    [TestCase("vertical-AliGn: SuPEr;", true)]
    [TestCase("vertical-align: test super test;", true)]
    [TestCase("vertical-align: super; vertical-align: test super test;font: line-through", true)]
    public void HasSuperscript_WithStyle_ShouldReturnMatch(string style, bool expectedMatch)
    {
        var isMatch = style.HasSuperscript();

        Assert.That(isMatch, Is.EqualTo(expectedMatch));
    }
}
