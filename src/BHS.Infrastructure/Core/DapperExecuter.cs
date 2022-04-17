using Dapper;
using System.Data;

namespace BHS.Infrastructure.Core
{
    public class DapperExecuter : IDbExecuter
    {
        private readonly ISqlConnectionFactory _factory;

        public DapperExecuter(ISqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<T?> ExecuteSprocQuerySingleOrDefault<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = await Open(connectionStringName);

            return await connection.QuerySingleOrDefaultAsync<T>(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> ExecuteSprocQuery<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = await Open(connectionStringName);

            return await connection.QueryAsync<T>(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> ExecuteSprocQueryMultiple<T1, T2>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = await Open(connectionStringName);

            using var multiResult = await connection.QueryMultipleAsync(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
            return (await multiResult.ReadAsync<T1>(), await multiResult.ReadAsync<T2>());
        }

        private async Task<IDbConnection> Open(string connectionStringName)
        {
            var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();
            return connection;
        }
    }
}
