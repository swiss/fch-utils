namespace Swiss.FCh.Utils.Models;

public class HtmlNormalizerOptions
{
    public bool RemoveClasses { get; set; }
    public bool RemoveStyles { get; set; }
    public bool RemoveImages { get; set; }
    public bool RemoveEmptyAnchors { get; set; }
    public bool RemoveSpans { get; set; }
    public bool ReplaceItalicWithEmphasis { get; set; }
    public bool ReplaceBoldWithStrong { get; set; }

    public static readonly HtmlNormalizerOptions Default = new()
    {
        RemoveClasses = true,
        RemoveStyles = true,
        RemoveImages = true,
        RemoveEmptyAnchors = true,
        RemoveSpans = true,
        ReplaceItalicWithEmphasis = true,
        ReplaceBoldWithStrong = true
    };
}
