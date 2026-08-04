namespace Swiss.FCh.Utils.Configurations;

/// <summary>
/// This class defines the options that are required to use the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/>.
/// </summary>
public class HtmlNormalizerOptions
{
    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> removes all "class" attributes from the HTML.
    /// </summary>
    public bool RemoveClasses { get; set; }

    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> removes all "style" attributes from the HTML.
    /// </summary>
    public bool RemoveStyles { get; set; }

    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> removes all "image" tags from the HTML.
    /// </summary>
    public bool RemoveImages { get; set; }

    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> removes all "a" tags that have no "href" attribute.
    /// </summary>
    public bool RemoveEmptyAnchors { get; set; }

    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> removes all "span" tags (the content of the span will be kept).
    /// </summary>
    public bool RemoveSpans { get; set; }

    /// <summary>
    /// If this option is enbled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> changes all "i" tags into "em" tags.
    /// </summary>
    public bool ReplaceItalicWithEmphasis { get; set; }

    /// <summary>
    /// If this option is enabled, the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> changes all "b" tags into "strong" tags.
    /// </summary>
    public bool ReplaceBoldWithStrong { get; set; }

    /// <summary>
    /// Default configuration for the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/>.
    /// </summary>
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
