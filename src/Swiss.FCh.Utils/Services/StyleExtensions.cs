using System.Text.RegularExpressions;

namespace Swiss.FCh.Utils.Services;

internal static partial class StyleExtensions
{
    public static bool HasUnderline(this string style)
    {
        return UnderlineRegex().IsMatch(style);
    }

    public static bool HasStrikeThrough(this string style)
    {
        return StrikeThroughRegex().IsMatch(style);
    }

    public static bool HasItalic(this string style)
    {
        return ItalicRegex().IsMatch(style);
    }

    public static bool HasBold(this string style)
    {
        return BoldRegex().IsMatch(style);
    }

    public static bool HasSubscript(this string style)
    {
        return SubscriptRegex().IsMatch(style);
    }

    public static bool HasSuperscript(this string style)
    {
        return SuperscriptRegex().IsMatch(style);
    }

    [GeneratedRegex(@"\btext-decoration:\s*(?:[\w-]+\s+)*underline\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex UnderlineRegex();
    [GeneratedRegex(@"\btext-decoration:\s*(?:[\w-]+\s+)*line-through\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex StrikeThroughRegex();
    [GeneratedRegex(@"\bfont-style:\s*(?:[\w-]+\s+)*italic\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex ItalicRegex();
    [GeneratedRegex(@"\bfont-weight:\s*(?:[\w-]+\s+)*bold\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex BoldRegex();
    [GeneratedRegex(@"\bvertical-align:\s*(?:[\w-]+\s+)*sub\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex SubscriptRegex();
    [GeneratedRegex(@"\bvertical-align:\s*(?:[\w-]+\s+)*super\s*;?", RegexOptions.IgnoreCase)]
    private static partial Regex SuperscriptRegex();
}
