using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;

namespace BHS.DataAccess.Core
{
    /// <remarks>
    /// In .NET 5, remember to <seealso cref="DbProviderFactories.RegisterFactory"/>
    /// the SqlClientFactory.Instance because
    /// there is no GAC or global configuration where providers would be automatically available.
    /// </remarks>
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private const string sqlProviderName = "System.Data.SqlClient";
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public SqlConnectionFactory(
            IConfiguration configuration,
            ILogger<SqlConnectionFactory> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public IDbConnection CreateConnection(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            IDbConnection connection = CreateDbConnection(sqlProviderName);
            connection.ConnectionString = connectionString;

            return connection;
        }

        private string GetConnectionString(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException(nameof(connectionStringName));

            string connectionString = _configuration?.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string {connectionStringName} is not configured.");

            return connectionString;
        }

        private IDbConnection CreateDbConnection(string providerInvariantName)
        {
            try
            {
                return DbProviderFactories.GetFactory(providerInvariantName).CreateConnection();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{0} is not registered.", providerInvariantName);
                throw;
            }
        }
    }
}
