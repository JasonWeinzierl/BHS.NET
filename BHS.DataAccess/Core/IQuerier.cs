using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Action to read data from <seealso cref="IDataReader"/> into instantiated model.
    /// </summary>
    /// <typeparam name="T"> Type of concrete model to fill. </typeparam>
    /// <param name="dr"> Data reader to read from. </param>
    /// <param name="model"> Model to fill. </param>
    public delegate void FillDelegate<T>(IDataReader dr, ref T model) where T : new();

    /// <summary>
    /// Function to get model from <seealso cref="IDataRecord"/>.
    /// </summary>
    /// <typeparam name="TModel"> Type of model to get from record. </typeparam>
    /// <param name="dr"> Data record to access column values from. </param>
    /// <returns> Instance of model resulting from record. </returns>
    public delegate TModel RecordDelegate<TModel>(IDataRecord dr);

    /// <summary>
    /// Action to read value(s) from output parameter(s) of a <seealso cref="IDbCommand"/>.
    /// </summary>
    /// <typeparam name="T"> Type of model to fill. </typeparam>
    /// <param name="command"> Command to read parameter values from. </param>
    /// <returns> Filled model. </returns>
    public delegate T ParameterDelegate<T>(IDbCommand command);

    /// <summary>
    /// Executes commands against a data source.
    /// </summary>
    public interface IQuerier
    {
        /// <summary>
        /// Execute command text and return a scalar.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="T"> Type of the returned value. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <returns> The first column of the first row in the resultset. </returns>
        Task<T> ExecuteScalarAsync<T>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand);

        /// <summary>
        /// Execute command text and read result.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="TOutput"> Type of model to fill with returned resultset. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="fillOutput"> Delegate called while the <seealso cref="IDataReader"/> returns true. </param>
        /// <returns> The output model filled from the resultset. </returns>
        Task<TOutput> ExecuteReaderAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, FillDelegate<TOutput> fillOutput) where TOutput : new();

        /// <summary>
        /// Execute command text and yield a model for each record.  By default, commandText is treated as a store procedure name.
        /// </summary>
        /// <typeparam name="TModel"> Type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="getModel"> Delegate to get model from each record. </param>
        /// <returns> Models filled from the resultset. </returns>
        IAsyncEnumerable<TModel> ExecuteReaderAsync<TModel>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, RecordDelegate<TModel> getModel);

        /// <summary>
        /// Execute command text and return the number of rows affected.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="transaction"> The transaction to be performed on the data source. </param>
        /// <returns> The number of rows affected. </returns>
        Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, IDbTransaction transaction = null);

        /// <summary>
        /// Execute command text and read output parameter(s).  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="configureCommand"> Action to configure the <seealso cref="IDbCommand"/>. This should be used to add parameters to the command. </param>
        /// <param name="readParameters"> Delegate called on <seealso cref="IDbCommand"/> after execution. </param>
        /// <returns> The output model filled from the output parameter(s). </returns>
        Task<TOutput> ExecuteNonQueryAsync<TOutput>(string connectionStringName, string commandText, Action<IDbCommand> configureCommand, ParameterDelegate<TOutput> readParameters);
    }
}
