using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BHS.DataAccess.Tests
{
    /// <remarks>
    /// Public writeable properties should be set to mock data.
    /// Public read-only properties should be used to assert repositories.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class MockQuerier : IQuerier
    {
        /// <summary>
        /// Gets or sets a value for QuerySingleOrDefault queries.
        /// </summary>
        /// <value>
        /// Single model to be returned.
        /// </value>
        public object SingleResult { get; set; }
        /// <summary>
        /// Gets or sets a value for Query queries.
        /// </summary>
        /// <value>
        /// Multiple models to be returned.
        /// </value>
        public IEnumerable<object> ManyResults { get; set; }
        /// <summary>
        /// Gets or sets a value for Scalar queries.
        /// </summary>
        /// <value>
        /// Object to be first cell's value.
        /// </value>
        public object ScalarCell { get; set; }
        /// <summary>
        /// Gets or sets a value for NonQuery queries.
        /// </summary>
        /// <value>
        /// Integer to be number of rows affected by NonQuery.
        /// </value>
        public int? NonQueryRowsAffected { get; set; }


        /// <summary>
        /// Gets name of connection string requested when creating latest connection.
        /// </summary>
        public string ConnectionStringName { get; private set; }
        /// <summary>
        /// Gets parameters bound to command.
        /// </summary>
        /// <remarks>
        /// The subject project being tested must have InternalsVisibleTo this test project
        /// in order for anonymous objects' members to be visible for assertions.
        /// Add an AssemblyAttribute to the csproj so it will be generated in the AssemblyInfo.cs.
        /// </remarks>
        public dynamic Parameters { get; private set; }
        /// <summary>
        /// Gets command text passed to connection.
        /// </summary>
        public string CommandText { get; private set; }


        public Task<T> ExecuteScalarAsync<T>(string connectionStringName, string commandText, object parameters = null)
        {
            if (ScalarCell is not T ret)
                throw new InvalidOperationException(nameof(ScalarCell) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult(ret);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string connectionStringName, string commandText, object parameters)
        {
            if (SingleResult is not T ret)
                throw new InvalidOperationException(nameof(SingleResult) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult(ret);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string connectionStringName, string commandText, object parameters = null)
        {
            if (ManyResults is not IEnumerable<T> ret)
                throw new InvalidOperationException(nameof(ManyResults) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult(ret);
        }

        public Task<int> ExecuteNonQueryAsync(string connectionStringName, string commandText, object parameters = null)
        {
            if (!NonQueryRowsAffected.HasValue)
                throw new InvalidOperationException(nameof(NonQueryRowsAffected) + " must have value.");

            ConnectionStringName = connectionStringName;
            CommandText = commandText;
            Parameters = parameters;

            return Task.FromResult(NonQueryRowsAffected.Value);
        }
    }
}
