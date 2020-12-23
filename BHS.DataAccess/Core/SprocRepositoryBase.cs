using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Base class for a repository using stored procedures.
    /// </summary>
    public abstract class SprocRepositoryBase
    {
        protected IQuerier Q { get; }

        public SprocRepositoryBase(IQuerier querier)
        {
            Q = querier;
        }

        #region CreateParameter
        /// <summary>
        /// Create <seealso cref="IDbDataParameter"/> for a <seealso cref="IDbCommand"/>.
        /// </summary>
        /// <remarks>
        /// Make sure to add the parameter to the command.
        /// </remarks>
        /// <param name="command"> Command to create new parameter from. </param>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <param name="value"> The object that is the value of the parameter. </param>
        /// <param name="dbType"> One of the <seealso cref="DbType"/> values.  The default is String. </param>
        /// <param name="direction"> One of the <seealso cref="ParameterDirection"/> values. The default is Input. </param>
        /// <returns> The created parameter. </returns>
        protected static IDbDataParameter CreateParameter(IDbCommand command, string parameterName, object value, DbType? dbType = null, ParameterDirection? direction = null)
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
        #endregion

        #region Parameter Converters
        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of string values
        /// to use with the StringList user-defined table type.
        /// </summary>
        /// <param name="strings"> Enumerable of strings. </param>
        /// <param name="columnName"> Name of column in the StringList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<string> strings, string columnName = "String")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(string));
            foreach (var s in strings)
                table.Rows.Add(s);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of int values
        /// to use with the IntList user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of ints. </param>
        /// <param name="columnName"> Name of column in the IntList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<int> numbers, string columnName = "Number")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(int));
            foreach (var n in numbers)
                table.Rows.Add(n);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of nullable int values
        /// to use with the NullableIntList user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of nullable ints. </param>
        /// <param name="columnName"> Name of column in the NullableIntList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<int?> numbers, string columnName = "Number")
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(int));
            column.AllowDBNull = true;
            foreach (var n in numbers)
                table.Rows.Add(n ?? (object)DBNull.Value);
            return table;
        }

        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of long values
        /// to use with the LongList user-defined table type.
        /// </summary>
        /// <param name="longs"> Enumerable of longs. </param>
        /// <param name="columnName"> Name of column in the LongList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        protected static DataTable ToDataTable(IEnumerable<long> longs, string columnName = "Long")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(long));
            foreach (var l in longs)
                table.Rows.Add(l);
            return table;
        }
        #endregion
    }
}
