using Swiss.FCh.Utils.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Swiss.FCh.Utils.Services;

public class HtmlNormalizer : IHtmlNormalizer
{
    private const string ClassAttributeName = "class";
    private const string StyleAttributeName = "style";
    private const string HrefAttributeName = "href";
    private const string ItalicElementName = "i";
    private const string EmphasisElementName = "em";
    private const string ImageElementName = "img";
    private const string AnchorElementName = "a";
    private const string BoldElementName = "b";
    private const string SpanElementName = "span";
    private const string StrongElementName = "strong";

    private readonly ILogger<HtmlNormalizer> _logger;

    public HtmlNormalizer(ILogger<HtmlNormalizer> logger)
    {
        _logger = logger;
    }

    public string? Normalize(string? html, HtmlNormalizerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return null;
        }

        options ??= HtmlNormalizerOptions.Default;

        var doc = new HtmlDocument();

        doc.LoadHtml(html);

        // Images
        if (options.RemoveImages)
        {
            var imageNodes = doc.DocumentNode.SelectNodes($"//{ImageElementName}") ?? Enumerable.Empty<HtmlNode>();
            foreach (var node in imageNodes)
            {
                _logger.LogInformation("Removing 'img' node from document");
                node.Remove();
            }
        }

        // Hyperlinks
        if (options.RemoveEmptyAnchors)
        {
            var anchorNodes = doc.DocumentNode.SelectNodes($"//{AnchorElementName}") ?? Enumerable.Empty<HtmlNode>();
            foreach (var node in anchorNodes)
            {
                var hrefValue = node.GetAttributeValue(HrefAttributeName, string.Empty);
                if (string.IsNullOrWhiteSpace(hrefValue))
                {
                    _logger.LogInformation("Replacing 'a' node with inner content");

                    // Move all child nodes of the anchor to its parent before removing the anchor itself
                    foreach (var childNode in node.ChildNodes)
                    {
                        node.ParentNode.InsertBefore(childNode, node);
                    }

                    node.Remove();
                }
            }
        }

        var nodes = (doc.DocumentNode.Descendants() ?? []).ToList();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            if (options.RemoveClasses)
            {
                _logger.LogInformation("Removing 'class' attribute on html element '{HtmlElement}'", node.Name);
                node.Attributes.Remove(ClassAttributeName);
            }

            if (options.RemoveStyles)
            {
                var style = node.GetAttributeValue(StyleAttributeName, string.Empty);
                node.Attributes.Remove(StyleAttributeName);
                HtmlNode? newNode = null;

                if (style.HasBold())
                {
                    var outerHtml = node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<strong>{outerHtml}</strong>");
                }

                if (style.HasUnderline())
                {
                    var outerHtml = newNode?.OuterHtml ?? node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<u>{outerHtml}</u>");
                }

                if (style.HasStrikeThrough())
                {
                    var outerHtml = newNode?.OuterHtml ?? node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<s>{outerHtml}</s>");
                }

                if (style.HasItalic())
                {
                    var outerHtml = newNode?.OuterHtml ?? node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<em>{outerHtml}</em>");
                }

                if (style.HasSubscript())
                {
                    var outerHtml = newNode?.OuterHtml ?? node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<sub>{outerHtml}</sub>");
                }

                if (style.HasSuperscript())
                {
                    var outerHtml = newNode?.OuterHtml ?? node.OuterHtml;
                    newNode = HtmlNode.CreateNode($"<sup>{outerHtml}</sup>");
                }

                if (newNode is not null)
                {
                    node.ParentNode.ReplaceChild(newNode, node);
                }

                _logger.LogInformation("Removing 'style' attribute on html element '{HtmlElement}'", node.Name);
            }

            if (options.ReplaceItalicWithEmphasis && node.OriginalName == ItalicElementName)
            {
                _logger.LogInformation("Replacing 'i' element with 'em' element");
                node.Name = EmphasisElementName;
            }

            if (options.ReplaceBoldWithStrong && node.OriginalName == BoldElementName)
            {
                _logger.LogInformation("Replacing 'b' element with 'strong' element");
                node.Name = StrongElementName;
            }
        }

        // Spans
        // NOTE: Only remove spans if all other options are enabled to avoid breaking formatting
        if (options is { RemoveSpans: true, RemoveStyles: true, ReplaceItalicWithEmphasis: true, ReplaceBoldWithStrong: true })
        {
            var anchorNodes = doc.DocumentNode.SelectNodes($"//{SpanElementName}") ?? Enumerable.Empty<HtmlNode>();
            foreach (var node in anchorNodes)
            {
                _logger.LogInformation("Replacing 'span' node with inner content");

                // Move all child nodes of the span to its parent before removing the span itself
                foreach (var childNode in node.ChildNodes)
                {
                    node.ParentNode.InsertBefore(childNode, node);
                }

                node.Remove();
            }
        }

        return doc.DocumentNode.InnerHtml;
    }
}
