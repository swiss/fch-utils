using Swiss.FCh.Utils.Configurations;
using Swiss.FCh.Utils.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Utils.Extensions;

/// <summary>
/// This class holds extension methods that allow adding and configuring features of <see cref="Swiss.FCh.Utils"/>.
/// </summary>
public static class UtilsServiceCollectionExtensions
{
    /// <summary>
    /// This extension method is a short-hand for adding, binding and validating options at the same time.
    /// </summary>
    /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> where the options should be added.</param>
    /// <param name="configuration">The <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> holding the configured optinos.</param>
    /// <param name="sectionKey">Name of the section in the appsettings.json file.</param>
    /// <typeparam name="T">Generic <see cref="System.Type"/> reflecting the structure of the options object.</typeparam>
    /// <returns>The bound <see cref="Microsoft.Extensions.Configuration.IConfigurationSection"/>.</returns>
    public static IConfigurationSection AddValidatedOptions<T>(this IServiceCollection services, IConfiguration configuration, string sectionKey) where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var optionsConfigSection = configuration.GetSection(sectionKey);

        services.AddOptions<T>()
            .Bind(optionsConfigSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return optionsConfigSection;
    }

    /// <summary>
    /// Adds the <see cref="Swiss.FCh.Utils.Services.IEmailService"/> (including configuration) to the DI container.
    /// </summary>
    /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> where the dependencies should be added.</param>
    /// <param name="configuration">The <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> holding the configured <see cref="Swiss.FCh.Utils.Configurations.EmailServiceOptions"/>.</param>
    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatedOptions<EmailServiceOptions>(configuration, EmailServiceOptions.SectionKey);

        services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
        services.AddScoped<IEmailService, EmailService>();
    }

    /// <summary>
    /// Ass the <see cref="Swiss.FCh.Utils.Services.IHtmlNormalizer"/> to the DI container.
    /// </summary>
    /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> where the dependencies should be added.</param>
    /// <param name="configuration">The <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> holding the configured <see cref="Swiss.FCh.Utils.Configurations.HtmlNormalizerOptions"/>.</param>
    public static void AddHtmlNormalizer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IHtmlNormalizer, HtmlNormalizer>();
    }
}
