using BHS.Infrastructure.Core;
using System.Diagnostics.CodeAnalysis;

namespace BHS.Infrastructure.Tests
{
    /// <remarks>
    /// Public writeable properties should be set to mock data.
    /// Public read-only properties should be used to assert repositories.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class MockExecuter : IDbExecuter
    {
        /// <summary>
        /// Gets or sets a value for QuerySingleOrDefault queries.
        /// </summary>
        /// <value>
        /// Single model to be returned.
        /// </value>
        public object? SingleResult { get; set; }
        /// <summary>
        /// Gets or sets a value for Query queries.
        /// </summary>
        /// <value>
        /// Multiple models to be returned.
        /// </value>
        public IEnumerable<object>? ManyResults { get; set; }
        /// <summary>
        /// Gets or sets a value for QueryMultiple{T1,T2} queries.
        /// </summary>
        /// <value>
        /// Result sets of multiple models to be returned.
        /// </value>
        public (IEnumerable<object> t1, IEnumerable<object> t2)? TwoManyResults { get; set; }


        /// <summary>
        /// Gets name of connection string requested when creating latest connection.
        /// </summary>
        public string? ConnectionStringName { get; private set; }
        /// <summary>
        /// Gets parameters bound to command.
        /// </summary>
        /// <remarks>
        /// The subject project being tested must have InternalsVisibleTo this test project
        /// in order for anonymous objects' members to be visible for assertions.
        /// Add an AssemblyAttribute to the csproj so it will be generated in the AssemblyInfo.cs.
        /// </remarks>
        public dynamic? Parameters { get; private set; }
        /// <summary>
        /// Gets command text passed to connection.
        /// </summary>
        public string? CommandText { get; private set; }


        public Task<T?> ExecuteSprocQuerySingleOrDefault<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            if (SingleResult is not T ret)
                throw new InvalidOperationException(nameof(SingleResult) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult((T?)ret);
        }

        public Task<IEnumerable<T>> ExecuteSprocQuery<T>(string connectionStringName, string commandText, object? parameters = null)
        {
            if (ManyResults is not IEnumerable<T> ret)
                throw new InvalidOperationException(nameof(ManyResults) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult(ret);
        }

        public Task<(IEnumerable<T1> resultset1, IEnumerable<T2> resultset2)> ExecuteSprocQueryMultiple<T1, T2>(string connectionStringName, string commandText, object? parameters = null)
        {
            if (!TwoManyResults.HasValue)
                throw new InvalidOperationException(nameof(TwoManyResults) + " must have value.");
            if (TwoManyResults.Value.t1 is not IEnumerable<T1> ret1 || TwoManyResults.Value.t2 is not IEnumerable<T2> ret2)
                throw new InvalidOperationException(nameof(TwoManyResults) + " t1 and t2 must each have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult((ret1, ret2));
        }
    }
}
