using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Extensions that convert a one-dimensional collection
    /// into Dapper's table-valued parameter so they can be passed to a stored procedure
    /// as a user-defined table type.
    /// </summary>
    public static class EnumerableToTVPExtensions
    {
        /// <summary>
        /// Create a Dapper table-valued parameter of string values
        /// to use with single-column user-defined table type.
        /// </summary>
        /// <param name="strings"> Enumerable of strings. </param>
        /// <param name="typeName"> Name of user-defined table type. </param>
        /// <param name="columnName"> Name of column in the user-defined table type. </param>
        /// <returns> A filled single-column table-valued parameter. </returns>
        public static SqlMapper.ICustomQueryParameter ToTableValuedParameter(this IEnumerable<string?> strings, string typeName = "StringList", string columnName = "String")
        {
            return ToDataTable(strings, columnName).AsTableValuedParameter(typeName);
        }

        internal static DataTable ToDataTable(IEnumerable<string?> strings, string columnName)
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(string));
            column.AllowDBNull = true;
            foreach (var s in strings)
                table.Rows.Add(s ?? Convert.DBNull);
            return table;
        }

        /// <summary>
        /// Create a Dapper table-valued parameter of int values
        /// to use with single-column user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of ints. </param>
        /// <param name="typeName"> Name of user-defined table type. </param>
        /// <param name="columnName"> Name of column in the user-defined table type. </param>
        /// <returns> A filled single-column table-valued parameter. </returns>
        public static SqlMapper.ICustomQueryParameter ToTableValuedParameter(this IEnumerable<int> numbers, string typeName = "IntList", string columnName = "Number")
        {
            return ToDataTable(numbers, columnName).AsTableValuedParameter(typeName);
        }

        internal static DataTable ToDataTable(IEnumerable<int> numbers, string columnName)
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(int));
            column.AllowDBNull = false;
            foreach (var n in numbers)
                table.Rows.Add(n);
            return table;
        }

        /// <summary>
        /// Create a Dapper table-valued parameter of nullable int values
        /// to use with single-column user-defined table type.
        /// </summary>
        /// <param name="numbers"> Enumerable of nullable ints. </param>
        /// <param name="typeName"> Name of user-defined table type. </param>
        /// <param name="columnName"> Name of column in the user-defined table type. </param>
        /// <returns> A filled single-column table-valued parameter. </returns>
        public static SqlMapper.ICustomQueryParameter ToTableValuedParameter(this IEnumerable<int?> numbers, string typeName = "NullableIntList", string columnName = "Number")
        {
            return ToDataTable(numbers, columnName).AsTableValuedParameter(typeName);
        }

        internal static DataTable ToDataTable(IEnumerable<int?> numbers, string columnName)
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(int));
            column.AllowDBNull = true;
            foreach (var n in numbers)
                table.Rows.Add(n ?? Convert.DBNull);
            return table;
        }

        /// <summary>
        /// Create a Dapper table-valued parameter of long values
        /// to use with single-column user-defined table type.
        /// </summary>
        /// <param name="longs"> Enumerable of longs. </param>
        /// <param name="typeName"> Name of user-defined table type. </param>
        /// <param name="columnName"> Name of column in the user-defined table type. </param>
        /// <returns> A filled single-column table-valued parameter. </returns>
        public static SqlMapper.ICustomQueryParameter ToTableValuedParameter(this IEnumerable<long> longs, string typeName = "LongList", string columnName = "Long")
        {
            return ToDataTable(longs, columnName).AsTableValuedParameter(typeName);
        }

        internal static DataTable ToDataTable(IEnumerable<long> longs, string columnName)
        {
            var table = new DataTable();
            var column = table.Columns.Add(columnName, typeof(long));
            column.AllowDBNull = false;
            foreach (var l in longs)
                table.Rows.Add(l);
            return table;
        }
    }
}
