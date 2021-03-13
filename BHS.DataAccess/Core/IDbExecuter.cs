using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Executes commands against a data source.
    /// </summary>
    public interface IDbExecuter
    {
        /// <summary>
        /// Execute command text and return a scalar.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="T"> Type of the returned value. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> The first column of the first row in the result set. </returns>
        Task<T?> ExecuteScalarAsync<T>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute command text and return a single model; throws on multiple.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <typeparam name="T"> Type of model to get from single record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> The output model filled from the result set. </returns>
        Task<T?> QuerySingleOrDefaultAsync<T>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute command text and return a model for each record.  By default, commandText is treated as a store procedure name.
        /// </summary>
        /// <typeparam name="T"> Type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> Models filled from the result set. </returns>
        Task<IEnumerable<T>> QueryAsync<T>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute command text and return two result sets with a model for each record.  By default, commandText is treated as a store procedure name.
        /// </summary>
        /// <typeparam name="T1"> First result set's type of model to get from each record. </typeparam>
        /// <typeparam name="T2"> Second result set's type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> Two collections of models filled from the two result sets. </returns>
        Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> QueryMultipleAsync<T1, T2>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute command text and return the number of rows affected.  By default, commandText is treated as a stored procedure name.
        /// </summary>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The text command to execute. By default, this is treated as the stored procedure name. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> The number of rows affected. </returns>
        Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, object? parameters = null);
    }
}
