using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    internal static class DataAsyncExtensions
    {
        /// <summary>
        /// Asynchronously opens a database connection with the settings
        /// specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        public static async Task OpenAsync(this IDbConnection self)
        {
            if (self is DbConnection dbConnection)
            {
                await dbConnection.OpenAsync();
            }
            else
            {
                await Task.Run(() => self.Open());
            }
        }

        /// <summary>
        /// Asynchronously executes the query, and returns the first column of the first row
        /// in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns> The first column of the first row in the resultset. </returns>
        public static async Task<object> ExecuteScalarAsync(this IDbCommand self)
        {
            if (self is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteScalarAsync();
            }
            else
            {
                return await Task.Run(() => self.ExecuteScalar());
            }
        }

        /// <summary>
        /// Asynchronously executes the <seealso cref="IDbCommand"/>.CommandText
        /// against the <seealso cref="IDbCommand"/>.Connection and builds an
        /// <seealso cref="IDataReader"/>.
        /// </summary>
        /// <returns> An <seealso cref="IDataReader"/> instance. </returns>
        public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand self)
        {
            if (self is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteReaderAsync();
            }
            else
            {
                return await Task.Run(() => self.ExecuteReader());
            }
        }

        /// <summary>
        /// Advances the <seealso cref="IDataReader"/> to the next record.
        /// </summary>
        /// <returns> True if there are more rows; otherwise, false. </returns>
        public static async Task<bool> ReadAsync(this IDataReader self)
        {
            if (self is DbDataReader dbDataReader)
            {
                return await dbDataReader.ReadAsync();
            }
            else
            {
                return await Task.Run(() => self.Read());
            }
        }

        /// <summary>
        /// Asynchronously executes the <seealso cref="IDbCommand"/>.CommandText
        /// against the <seealso cref="IDbCommand"/>.Connection, and returns the number of rows affected.
        /// </summary>
        /// <returns> The number of rows affected. </returns>
        public static async Task<int> ExecuteNonQueryAsync(this IDbCommand self)
        {
            if (self is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteNonQueryAsync();
            }
            else
            {
                return await Task.Run(() => self.ExecuteNonQuery());
            }
        }
    }
}
