using Dapper;
using System.Data;

namespace BHS.Infrastructure.Core;

public class DapperExecuter : IDbExecuter
{
    private readonly ISqlConnectionFactory _factory;

    public DapperExecuter(ISqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<T?> ExecuteSprocQuerySingleOrDefault<T>(string connectionStringName, string commandText, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using var connection = await Open(connectionStringName, cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(
            commandText: commandText,
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<T>> ExecuteSprocQuery<T>(string connectionStringName, string commandText, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using var connection = await Open(connectionStringName, cancellationToken);

        return await connection.QueryAsync<T>(new CommandDefinition(
            commandText: commandText,
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> ExecuteSprocQueryMultiple<T1, T2>(string connectionStringName, string commandText, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using var connection = await Open(connectionStringName, cancellationToken);

        using var multiResult = await connection.QueryMultipleAsync(new CommandDefinition(
            commandText: commandText,
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return (await multiResult.ReadAsync<T1>(), await multiResult.ReadAsync<T2>());
    }

    private async Task<IDbConnection> Open(string connectionStringName, CancellationToken cancellationToken)
    {
        var connection = _factory.CreateConnection(connectionStringName);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
