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
                await Task.Run(self.Open);
            }
        }
    }
}
