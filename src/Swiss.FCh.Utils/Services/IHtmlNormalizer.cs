using Swiss.FCh.Utils.Configurations;

namespace Swiss.FCh.Utils.Services;

public interface IHtmlNormalizer
{
    string? Normalize(string? html, HtmlNormalizerOptions? options = null);
}
