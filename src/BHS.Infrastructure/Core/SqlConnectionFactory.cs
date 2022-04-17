using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace BHS.Infrastructure.Core
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger? _logger;

        public SqlConnectionFactory(
            IConfiguration configuration,
            ILogger<SqlConnectionFactory>? logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public DbConnection CreateConnection(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentException("Connection string name is null or empty.", nameof(connectionStringName));

            string connectionString = GetConnectionString(connectionStringName);

            return CreateSqlConnection(connectionString);
        }

        private string GetConnectionString(string connectionStringName)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string {connectionStringName} is not configured.");

            return connectionString;
        }

        private DbConnection CreateSqlConnection(string connectionString)
        {
            try
            {
                var connection = DbProviderFactories.GetFactory(DbConstants.SqlClientProviderName).CreateConnection()
                    ?? throw new InvalidOperationException("Provider factory returned null connection.");

                connection.ConnectionString = connectionString;

                return connection;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to create connection to database.  Is the provider factory '{providerName}' registered?  {message}", DbConstants.SqlClientProviderName, ex.Message);
                throw;
            }
        }
    }
}
