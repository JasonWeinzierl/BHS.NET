using System.Data.Common;

namespace BHS.DataAccess.Core
{
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates a connection based on the given database connection string name.
        /// </summary>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <returns> An initialized SqlConnection. </returns>
        DbConnection CreateConnection(string connectionStringName);
    }
}
