using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace BHS.DataAccess.Core
{
    internal static class DataAsyncExtensions
    {
        /// <summary>
        /// Asynchronously opens a database connection with the settings
        /// specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        public static async Task OpenAsync(this IDbConnection self, CancellationToken cancellationToken = default)
        {
            if (self is DbConnection dbConnection)
            {
                await dbConnection.OpenAsync(cancellationToken);
            }
            else
            {
                await Task.Run(self.Open, cancellationToken);
            }
        }
    }
}
