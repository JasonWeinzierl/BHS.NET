using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Extensions that convert a one-dimensional collection
    /// into a DataTable so they can be passed to a stored procedure
    /// as a user-defined table type.
    /// </summary>
    public static class EnumerableToDataTableExtensions
    {
        /// <summary>
        /// Create a <seealso cref="DataTable"/> parameter of string values
        /// to use with the StringList user-defined table type.
        /// </summary>
        /// <param name="strings"> Enumerable of strings. </param>
        /// <param name="columnName"> Name of column in the StringList user-defined table type. </param>
        /// <returns> A filled single-column <seealso cref="DataTable"/>. </returns>
        public static DataTable ToDataTable(this IEnumerable<string> strings, string columnName = "String")
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
        public static DataTable ToDataTable(this IEnumerable<int> numbers, string columnName = "Number")
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
        public static DataTable ToDataTable(this IEnumerable<int?> numbers, string columnName = "Number")
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
        public static DataTable ToDataTable(this IEnumerable<long> longs, string columnName = "Long")
        {
            var table = new DataTable();
            table.Columns.Add(columnName, typeof(long));
            foreach (var l in longs)
                table.Rows.Add(l);
            return table;
        }
    }
}
