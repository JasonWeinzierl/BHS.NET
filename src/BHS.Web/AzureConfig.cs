using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace BHS.Web;

internal static class AzureConfig
{
    /// <summary>
    /// Adds Azure App Configuration data for the current environment.
    /// </summary>
    public static void AddAzureAppConfiguration(this IConfigurationBuilder cfbBuilder, HostBuilderContext context)
    {
        IConfiguration applicationConfig = cfbBuilder.Build(); // Only necessary with the old bootstrap process.  WebApplication.CreateBuilder's Configuration doesn't need this.
        string? connectionString = applicationConfig.GetConnectionString("AppConfig");

        if (connectionString is null)
        {
            Console.Error.WriteLine("Azure App Configuration connection string is null, so data will not be loaded.");
            return;
        }

        cfbBuilder.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(connectionString)
                // Use the default credential which aggregates several different credential types.
                .ConfigureKeyVault(kvOptions => kvOptions.SetCredential(new DefaultAzureCredential()))
                // Load configuration values with no label.
                .Select(KeyFilter.Any, LabelFilter.Null)
                // Override with any configuration values specific to current hosting environment.
                .Select(KeyFilter.Any, context.HostingEnvironment.EnvironmentName.ToLowerInvariant());
        });
    }
}
