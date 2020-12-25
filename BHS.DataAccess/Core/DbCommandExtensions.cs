using System;
using System.Data;

namespace BHS.DataAccess.Core
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Builds an <seealso cref="IDbDataParameter"/> for an <seealso cref="IDbCommand"/>
        /// with provided arguments.
        /// </summary>
        /// <remarks>
        /// Does not add the parameter to the command.
        /// </remarks>
        /// <param name="command"> Command to create new parameter from. </param>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <param name="value"> The object that is the value of the parameter. </param>
        /// <param name="dbType"> One of the <seealso cref="DbType"/> values.  The default is String. </param>
        /// <param name="direction"> One of the <seealso cref="ParameterDirection"/> values. The default is Input. </param>
        /// <returns> The created parameter. </returns>
        public static IDbDataParameter CreateParameter(this IDbCommand command, string parameterName, object value, DbType? dbType = null, ParameterDirection? direction = null)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            if (dbType.HasValue)
                parameter.DbType = dbType.Value;

            if (direction.HasValue)
                parameter.Direction = direction.Value;

            return parameter;
        }
    }
}
