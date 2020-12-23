using System;
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

        public async Task<T> ExecuteScalarAsync<T>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            return (T)await command.ExecuteScalarAsync();
        }


        public async Task<TOutput> ExecuteReaderAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, FillDelegate<TOutput> fillOutput) where TOutput : new()
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            using var reader = await command.ExecuteReaderAsync();
            var outputModel = new TOutput();
            while (await reader.ReadAsync())
            {
                fillOutput(reader, ref outputModel);
            }

            return outputModel;
        }

        public async IAsyncEnumerable<TModel> ExecuteReaderAsync<TModel>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, RecordDelegate<TModel> getModel)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return getModel(reader);
            }
        }


        public async Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, IDbTransaction transaction = null)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();
            return await ExecuteNonQueryAsync(connection, commandText, configureCommand, transaction);
        }

        /// <summary>
        /// Executes command text on an open connection.  Does not automatically close connection.
        /// </summary>
        /// <remarks>
        /// Make sure to close the connection.
        /// </remarks>
        /// <param name="connection"> The open connection. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="transaction"> The transaction to be performed on the data source. </param>
        /// <returns> The number of rows affected. </returns>
        internal static Task<int> ExecuteNonQueryAsync(IDbConnection connection, string commandText, Action<IDbCommand> configureCommand, IDbTransaction transaction)
        {
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);
            if (transaction != null)
                command.Transaction = transaction;

            return command.ExecuteNonQueryAsync();
        }

        public async Task<TOutput> ExecuteNonQueryAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, ParameterDelegate<TOutput> readParameters)
        {
            using var connection = _factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            _ = await command.ExecuteNonQueryAsync();
            return readParameters(command);
        }
    }
}
