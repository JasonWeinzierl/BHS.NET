using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    public class DapperExecuter : IDbExecuter
    {
        private readonly IDbConnectionFactory _factory;

        public DapperExecuter(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<T?> ExecuteScalarAsync<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.ExecuteScalarAsync<T>(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.QuerySingleOrDefaultAsync<T>(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.QueryAsync<T>(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> QueryMultipleAsync<T1, T2>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var multiResult = await connection.QueryMultipleAsync(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
            return (await multiResult.ReadAsync<T1>(), await multiResult.ReadAsync<T2>());
        }

        public async Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2, IEnumerable<T3> resultset3)> QueryMultipleAsync<T1, T2, T3>(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var multiResult = await connection.QueryMultipleAsync(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
            return (await multiResult.ReadAsync<T1>(), await multiResult.ReadAsync<T2>(), await multiResult.ReadAsync<T3>());
        }

        public async Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, object? parameters = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.ExecuteAsync(
                commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
