using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Swiss.FCh.Utils.Rhos.Extensions;

public static class ConfigurationExtensions
{
    public static IHostApplicationBuilder AddRhosConfigurations(this IHostApplicationBuilder builder, string stageEnvVarName = "STAGE")
    {
        ArgumentNullException.ThrowIfNull(builder);

        var stage = Environment.GetEnvironmentVariable(stageEnvVarName);

        if (Stage.IsRhosStage(stage))
        {
            Log.Information("RHOS stage={Stage} detected, adding environment variables to configuration", stage);

            // Configuration that is not secret will come from the config map in the kubernetes deployment
            // and will be provided as environment variables matching the appsettings.json
            builder.Configuration.AddEnvironmentVariables();
        }

        return builder;
    }

    public static IHostApplicationBuilder AddRhosPostgresConfiguration(
        this IHostApplicationBuilder builder,
        string stageEnvVarName = "STAGE",
        string vaultPath = "../vault/secrets/pg-database-credentials.json")
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(vaultPath);

        AddRhosJsonConfiguration(builder, vaultPath, stageEnvVarName);

        return builder;
    }

    public static IHostApplicationBuilder AddRhosS3Configuration(
        this IHostApplicationBuilder builder,
        string stageEnvVarName = "STAGE",
        string vaultPath = "../vault/secrets/s3-credentials.json")
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(vaultPath);

        AddRhosJsonConfiguration(builder, vaultPath, stageEnvVarName);

        return builder;
    }

    /// <summary>
    /// Get the postgres connection string from the configuration.
    /// Make sure to call <see cref="AddRhosPostgresConfiguration"/> first.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetPostgresConnectionString(this IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("pgsql:hostname");
        var port = configuration.GetValue<string>("pgsql:port");
        var user = configuration.GetValue<string>("pgsql:username");
        var pass = configuration.GetValue<string>("pgsql:password");
        var database = configuration.GetValue<string>("pgsql:database_name");
        var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={database};";
        return connectionString;
    }

    private static void AddRhosJsonConfiguration(IHostApplicationBuilder builder, string vaultPath, string stageEnvVarName = "STAGE")
    {
        var stage = Environment.GetEnvironmentVariable(stageEnvVarName);
        if (Stage.IsRhosStage(stage))
        {
            Log.Information("RHOS stage={Stage} detected, adding json file {JSONFile}", stage, vaultPath);

            if (vaultPath.StartsWith("..", StringComparison.InvariantCultureIgnoreCase)) //secrets files are outside of the applications scope
            {
                // These files are placed in the pod by the vault agent, and they match the secrets of our appsettings.json file
                // ConfigurationBuilder.AddJsonFile will NOT load files that are above the root directory, hence the stream reading...
                var stream = File.OpenRead(vaultPath);

                builder.Configuration.AddJsonStream(stream);

                stream.Dispose();
            }
            else
            {
                builder.Configuration.AddJsonFile(vaultPath); //files inside the applications scope can be loaded "normally"
            }
        }
    }
}
