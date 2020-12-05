using System.Data;

namespace BHS.DataAccess.Core
{
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates a connection based on the given database connection string name.
        /// </summary>
        /// <param name="connectionStringName"> The database connection string name. </param>
        /// <returns> An initialized <seealso cref="IDbConnection"/>. </returns>
        IDbConnection CreateConnection(string connectionStringName);
    }
}
