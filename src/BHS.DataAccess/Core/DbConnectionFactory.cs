using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;

namespace BHS.DataAccess.Core
{
    /// <remarks>
    /// In .NET Core, remember to <seealso cref="DbProviderFactories.RegisterFactory"/>
    /// the DbProviderFactory instance (e.g. SqlClientFactory.Instance) because
    /// there is no GAC or global configuration where providers would be automatically available.
    /// </remarks>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger? _logger;

        private const string providerConnectionStringNameFormat = "{0}_ProviderName";

        public DbConnectionFactory(
            IConfiguration configuration,
            ILogger<DbConnectionFactory>? logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public IDbConnection CreateConnection(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException(nameof(connectionStringName));

            string connectionString = GetConnectionString(connectionStringName);
            string providerName = GetProviderName(connectionStringName);

            IDbConnection connection = CreateDbConnection(providerName);
            connection.ConnectionString = connectionString;

            return connection;
        }

        private string GetConnectionString(string connectionStringName)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string {connectionStringName} is not configured.");

            return connectionString;
        }

        private string GetProviderName(string connectionStringName)
        {
            string providerConnectionStringName = string.Format(providerConnectionStringNameFormat, connectionStringName);

            string providerName = _configuration.GetConnectionString(providerConnectionStringName);

            if (string.IsNullOrEmpty(providerName))
                throw new InvalidOperationException($"Connection string {connectionStringName} does not have a provider configured." +
                    $"It should be a connection string named {providerConnectionStringName}");

            return providerName;
        }

        private IDbConnection CreateDbConnection(string providerInvariantName)
        {
            try
            {
                return DbProviderFactories.GetFactory(providerInvariantName).CreateConnection()
                    ?? throw new InvalidOperationException("Provider factory returned null connection.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to create connection to database.  Is the provider factory '{0}' registered?  {1}", providerInvariantName, ex.Message);
                throw;
            }
        }
    }
}
