using Swiss.FCh.Utils.Configurations;
using Swiss.FCh.Utils.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Utils.Extensions;

public static class UtilsServiceCollectionExtensions
{
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

    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatedOptions<EmailServiceOptions>(configuration, EmailServiceOptions.SectionKey);

        services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
        services.AddScoped<IEmailService, EmailService>();
    }

    public static void AddHtmlNormalizer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IHtmlNormalizer, HtmlNormalizer>();
    }
}
