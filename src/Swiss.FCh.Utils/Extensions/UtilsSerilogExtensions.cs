using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;


namespace Swiss.FCh.Utils.Extensions;

/// <summary>
/// This class holds the extension method to configure Serilog in your application.
/// </summary>
public static class UtilsSerilogExtensions
{
    /// <summary>
    /// Creates a logger and injects it in the CI container to be used by your application.
    /// </summary>
    /// <param name="builder">The <see cref="Microsoft.Extensions.Hosting.IHostApplicationBuilder"/> holding the DI container (service collection) where the logger should be added.</param>
    /// <param name="applicationName">The name of the application will be added to the structured logs (property name "app").</param>
    /// <returns>The <see cref="Microsoft.Extensions.Hosting.IHostApplicationBuilder"/> for forther usage in fluent syntax.</returns>
    public static IHostApplicationBuilder AddSerilog(this IHostApplicationBuilder builder, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var logger = CreateLogger(applicationName);
        builder.Services.AddSerilog();
        Log.Logger = logger;
        return builder;
    }

    private static Serilog.Core.Logger CreateLogger(string applicationName)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{env}.json", true)
            .AddEnvironmentVariables()
            .Build();

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadName()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("app", applicationName)
            .CreateLogger();
    }
}
