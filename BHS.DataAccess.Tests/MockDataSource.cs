using BHS.DataAccess.Core;
using Moq;
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
        // todo: throw if these are null and needed
        /// <summary>
        /// Resultset returned from Reader query.
        /// </summary>
        public DataTable ReaderResultset { get; set; }
        /// <summary>
        /// Cell returned from Scalar query.
        /// </summary>
        public object ScalarCell { get; set; }
        /// <summary>
        /// Number of rows affected returned from NonQuery.
        /// </summary>
        public int NonQueryRowsAffected { get; set; }


        /// <summary>
        /// Name of connection string requested when creating latest connection.
        /// </summary>
        public string ConnectionStringName { get; private set; }
        /// <summary>
        /// List of parameters bound to mock command.
        /// </summary>
        public IList<IDbDataParameter> Parameters { get; private set; }
        /// <summary>
        /// Command text assigned to mock command.
        /// </summary>
        public string CommandText { get; private set; }
        /// <summary>
        /// Command type set to mock command.
        /// </summary>
        public CommandType CommandType { get; private set; }


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
                .Returns(() => ReaderResultset.CreateDataReader());
            cmd.Setup(c => c.ExecuteScalar())
                .Returns(() => ScalarCell);
            cmd.Setup(c => c.ExecuteNonQuery())
                .Returns(() => NonQueryRowsAffected);

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
