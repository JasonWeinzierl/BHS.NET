using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    public class Querier : IQuerier
    {
        private readonly IDbConnectionFactory _factory;

        public Querier(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<T> ExecuteScalarAsync<T>(string connectionStringName, string commandText, object parameters)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.ExecuteScalarAsync<T>(commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<T> QuerySingleOrDefaultAsync<T>(string connectionStringName, string commandText, object parameters)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.QuerySingleOrDefaultAsync<T>(commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionStringName, string commandText, object parameters)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.QueryAsync<T>(commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, object parameters)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            return await connection.ExecuteAsync(commandText,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
