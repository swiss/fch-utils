using Swiss.FCh.Utils.Models;

namespace Swiss.FCh.Utils.Services;

public interface IHtmlNormalizer
{
    string? Normalize(string? html, HtmlNormalizerOptions? options = null);
}
