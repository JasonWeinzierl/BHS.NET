using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Base class for a repository using stored procedures.
    /// </summary>
    public abstract class SprocRepositoryBase
    {
        protected IDbConnectionFactory Factory { get; }

        public SprocRepositoryBase(IDbConnectionFactory factory)
        {
            Factory = factory;
        }

        #region CreateParameter
        /// <summary>
        /// Create <seealso cref="IDbDataParameter"/> for a <seealso cref="IDbCommand"/>.
        /// </summary>
        /// <remarks>
        /// Make sure to add the parameter to the command.
        /// </remarks>
        /// <param name="command"> Command to create new parameter from. </param>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <param name="value"> The object that is the value of the parameter. </param>
        /// <param name="dbType"> One of the <seealso cref="DbType"/> values.  The default is String. </param>
        /// <param name="direction"> One of the <seealso cref="ParameterDirection"/> values. The default is Input. </param>
        /// <returns> The created parameter. </returns>
        protected static IDbDataParameter CreateParameter(IDbCommand command, string parameterName, object value, DbType? dbType = null, ParameterDirection? direction = null)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            if (dbType.HasValue)
                parameter.DbType = dbType.Value;

            if (direction.HasValue)
                parameter.Direction = direction.Value;

            return parameter;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// Execute command text and return a scalar.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="T"> Type of the returned value. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <returns> The first column of the first row in the resultset. </returns>
        protected async Task<T> ExecuteScalarAsync<T>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand)
        {
            using var connection = Factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            return (T)await command.ExecuteScalarAsync();
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// Action to read data from <seealso cref="IDataReader"/> into instantiated model.
        /// </summary>
        /// <typeparam name="T"> Type of concrete model to fill. </typeparam>
        /// <param name="dr"> Data reader to read from. </param>
        /// <param name="model"> Model to fill. </param>
        protected delegate void FillDelegate<T>(IDataReader dr, ref T model) where T : new();

        /// <summary>
        /// Execute command text and read result.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="TOutput"> Type of model to fill with returned resultset. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="fillOutput"> Delegate called while the <seealso cref="IDataReader"/> returns true. </param>
        /// <returns> The output model filled from the resultset. </returns>
        protected async Task<TOutput> ExecuteReaderAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, FillDelegate<TOutput> fillOutput) where TOutput : new()
        {
            using var connection = Factory.CreateConnection(connectionStringName);
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

        /// <summary>
        /// Function to get model from <seealso cref="IDataRecord"/>.
        /// </summary>
        /// <typeparam name="TModel"> Type of model to get from record. </typeparam>
        /// <param name="dr"> Data record to access column values from. </param>
        /// <returns> Instance of model resulting from record. </returns>
        protected delegate TModel RecordDelegate<TModel>(IDataRecord dr);

        /// <summary>
        /// Execute command text and yield a model for each record.  By default, commandText is treated as a store procedure name.
        /// </summary>
        /// <typeparam name="TModel"> Type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="getModel"> Delegate to get model from each record. </param>
        /// <returns> Models filled from the resultset. </returns>
        protected async IAsyncEnumerable<TModel> ExecuteReaderAsync<TModel>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, RecordDelegate<TModel> getModel)
        {
            using var connection = Factory.CreateConnection(connectionStringName);
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
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Execute command text and return the number of rows affected.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="transaction"> The transaction to be performed on the data source. </param>
        /// <returns> The number of rows affected. </returns>
        protected async Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, IDbTransaction transaction = null)
        {
            using var connection = Factory.CreateConnection(connectionStringName);
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

        /// <summary>
        /// Action to read value(s) from output parameter(s) of a <seealso cref="IDbCommand"/>.
        /// </summary>
        /// <typeparam name="T"> Type of model to fill. </typeparam>
        /// <param name="command"> Command to read parameter values from. </param>
        /// <returns> Filled model. </returns>
        protected delegate T ParameterDelegate<T>(IDbCommand command);

        /// <summary>
        /// Execute command text and read output parameter(s).  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="readParameters"> Delegate called on <seealso cref="IDbCommand"/> after execution. </param>
        /// <returns> The output model filled from the output parameter(s). </returns>
        protected async Task<TOutput> ExecuteNonQueryAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, ParameterDelegate<TOutput> readParameters)
        {
            using var connection = Factory.CreateConnection(connectionStringName);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            configureCommand?.Invoke(command);

            _ = await command.ExecuteNonQueryAsync();
            return readParameters(command);
        }
        #endregion

        #region Parameter Converters
        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of string values
        /// to use with the StringList user-defined table type.
        /// </summary>
        /// <param name="strings"> Enumerable of strings. </param>
        /// <param name="columnName"> Name of column in the StringList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<string> strings, string columnName = "String")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(string));
            foreach (var s in strings)
                table.Rows.Add(s);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of int values
        /// to use with the IntList user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of ints. </param>
        /// <param name="columnName"> Name of column in the IntList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<int> numbers, string columnName = "Number")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(int));
            foreach (var n in numbers)
                table.Rows.Add(n);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of nullable int values
        /// to use with the NullableIntList user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of nullable ints. </param>
        /// <param name="columnName"> Name of column in the NullableIntList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<int?> numbers, string columnName = "Number")
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(int));
            column.AllowDBNull = true;
            foreach (var n in numbers)
                table.Rows.Add(n ?? (object)DBNull.Value);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of long values
        /// to use with the LongList user-defined table type.
        /// </summary>
        /// <param name="longs"> Enumerable of longs. </param>
        /// <param name="columnName"> Name of column in the LongList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<long> longs, string columnName = "Long")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(long));
            foreach (var l in longs)
                table.Rows.Add(l);
            return table;
        }
        #endregion
    }
}
