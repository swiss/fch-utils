using Swiss.FCh.Utils.Configurations;

namespace Swiss.FCh.Utils.Services;

/// <summary>
/// This HTML normalizer can remove certain (unwanted) elements or attributes from an HTML content.
/// In practice, it is used to reduce the HTML to a subset of the actual HTML capabilities, to meet data-contracts between different systems in the federal administration.
///
/// WARNING: the normalizer is not a security feature and must not be used for sanitizing HTML.
/// </summary>
public interface IHtmlNormalizer
{
    /// <summary>
    /// Normalizes the HTML according to the options provided.
    /// </summary>
    /// <param name="html">The HTML as <see langword="string"/></param>
    /// <param name="options">Options to configure the behavior of the normalizer</param>
    /// <returns></returns>
    string? Normalize(string? html, HtmlNormalizerOptions? options = null);
}
