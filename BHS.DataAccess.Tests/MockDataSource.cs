using BHS.DataAccess.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BHS.DataAccess.Tests
{
    /// <summary>
    /// Creates mocks for the data layer's underlying source.
    /// Public writeable properties should be set to mock data.
    /// Public read-only properties should be used to assert desired functionality
    /// of repositories using Core.
    /// </summary>
    public class MockDataSource
    {
        /// <summary>
        /// Gets or sets a value for Reader queries.
        /// </summary>
        /// <value>
        /// DataTable of flattened model(s) to be resultset.
        /// </value>
        public DataTable ReaderResultset { get; set; }
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
        /// Gets list of parameters bound to mock command.
        /// </summary>
        public IList<IDbDataParameter> Parameters { get; private set; }
        /// <summary>
        /// Gets command text assigned to mock command.
        /// </summary>
        public string CommandText { get; private set; }
        /// <summary>
        /// Gets command type set to mock command.
        /// </summary>
        public CommandType CommandType { get; private set; }


        /// <summary>
        /// Sets resultset for Reader queries.
        /// Columns are defined by public properties,
        /// and each model will be a row.
        /// </summary>
        /// <typeparam name="T"> Type of model to put in each row. </typeparam>
        /// <param name="models"> Models to be loaded into the resultset. </param>
        public void SetReaderResultset<T>(IEnumerable<T> models)
        {
            ReaderResultset = CreateResultset(models);
        }

        /// <summary>
        /// Create DataTable from models.
        /// Columns are defined by public properties,
        /// and each model will be a row.
        /// </summary>
        /// <typeparam name="T"> Type of model to put in each row. </typeparam>
        /// <param name="models"> Models to be loaded into a DataTable. </param>
        /// <returns> A DataTable loaded with the models. </returns>
        public static DataTable CreateResultset<T>(IEnumerable<T> models)
        {
            var properties = typeof(T).GetProperties();

            var table = new DataTable();
            foreach (var propertyInfo in properties)
            {
                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }

            foreach (var model in models)
            {
                var values = properties.Select(propertyInfo => propertyInfo.GetValue(model));

                table.Rows.Add(values.ToArray());
            }

            return table;
        }

        /// <summary>
        /// Creates mock factory that creates mock connections.
        /// </summary>
        public Mock<IDbConnectionFactory> CreateDbConnectionFactory()
        {
            var connFactory = new Mock<IDbConnectionFactory>();

            connFactory.Setup(f => f.CreateConnection(It.IsAny<string>()))
                .Returns(CreateConnection().Object)
                .Callback<string>(connStrName => ConnectionStringName = connStrName);

            return connFactory;
        }

        /// <summary>
        /// Creates mock database connection which creates mock commands.
        /// </summary>
        private Mock<IDbConnection> CreateConnection()
        {
            var conn = new Mock<IDbConnection>();

            conn.Setup(c => c.Open());
            conn.Setup(c => c.CreateCommand())
                .Returns(CreateCommand().Object);
            conn.Setup(c => c.Dispose());

            return conn;
        }

        /// <summary>
        /// Creates mock db command which returns mock datareaders and mock parameter collections.
        /// </summary>
        private Mock<IDbCommand> CreateCommand()
        {
            Parameters = new List<IDbDataParameter>();

            var cmd = new Mock<IDbCommand>();

            cmd.Setup(c => c.Parameters)
                .Returns(CreateParameterCollection().Object);
            cmd.Setup(c => c.CreateParameter())
                .Returns(() => new MockDbDataParameter());

            cmd.Setup(c => c.ExecuteReader())
                .Returns(() => ReaderResultset?.CreateDataReader() ?? throw new InvalidOperationException(nameof(ReaderResultset) + " must have value."));
            cmd.Setup(c => c.ExecuteScalar())
                .Returns(() => ScalarCell ?? throw new InvalidOperationException(nameof(ScalarCell) + " must have value."));
            cmd.Setup(c => c.ExecuteNonQuery())
                .Returns(() => NonQueryRowsAffected ?? throw new InvalidOperationException(nameof(NonQueryRowsAffected) + " must have value."));

            cmd.SetupSet(c => c.CommandText = It.IsAny<string>())
                .Callback<string>(cmdText => CommandText = cmdText);
            cmd.SetupGet(c => c.CommandText)
                .Returns(() => CommandText);

            cmd.SetupSet(c => c.CommandType = It.IsAny<CommandType>())
                .Callback<CommandType>(cmdType => CommandType = cmdType);
            cmd.SetupGet(c => c.CommandType)
                .Returns(() => CommandType);

            return cmd;
        }

        /// <summary>
        /// Creates mock paramater collection.
        /// </summary>
        private Mock<IDataParameterCollection> CreateParameterCollection()
        {
            var paramCollection = new Mock<IDataParameterCollection>();

            paramCollection.Setup(c => c.Add(It.IsAny<object>()))
                .Callback<object>(value => Parameters.Add((IDbDataParameter)value));
            // Used for output parameters.
            paramCollection.Setup(c => c[It.IsAny<string>()])
                .Returns<string>(parameterName => Parameters.FirstOrDefault(p => p.ParameterName.Equals(parameterName)));

            return paramCollection;
        }
    }
}
