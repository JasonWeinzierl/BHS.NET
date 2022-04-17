namespace BHS.Infrastructure.Core
{
    /// <summary>
    /// Executes stored procedures against a data source.
    /// </summary>
    public interface IDbExecuter
    {
        /// <summary>
        /// Execute stored procedure and return a single model; throws on multiple.
        /// </summary>
        /// <typeparam name="T"> Type of model to get from single record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The stored procedure to execute. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> The output model filled from the result set. </returns>
        Task<T?> ExecuteSprocQuerySingleOrDefault<T>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute stored procedure and return a model for each record.
        /// </summary>
        /// <typeparam name="T"> Type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The stored procedure to execute. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> Models filled from the result set. </returns>
        Task<IEnumerable<T>> ExecuteSprocQuery<T>(string connectionStringName, string commandText, object? parameters = null);

        /// <summary>
        /// Execute stored procedure and return two result sets with a model for each record.
        /// </summary>
        /// <typeparam name="T1"> First result set's type of model to get from each record. </typeparam>
        /// <typeparam name="T2"> Second result set's type of model to get from each record. </typeparam>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <param name="commandText"> The stored procedure to execute. </param>
        /// <param name="parameters"> Object of parameter values. </param>
        /// <returns> Two collections of models filled from the two result sets. </returns>
        Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> ExecuteSprocQueryMultiple<T1, T2>(string connectionStringName, string commandText, object? parameters = null);
    }
}
