using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;


namespace Swiss.FCh.Utils.Extensions;

public static class UtilsSerilogExtensions
{
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
