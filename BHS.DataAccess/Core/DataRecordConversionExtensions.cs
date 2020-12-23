using System;
using System.Data;

namespace BHS.DataAccess.Core
{
    /// <summary>
    /// Extensions that convert cell value to a type.
    /// Unlike the getters of <seealso cref="IDataRecord"/>,
    /// these return default if null.
    /// </summary>
    public static class DataRecordConversionExtensions
    {
        /// <summary>
        /// Cast cell to long.
        /// </summary>
        public static long CastLong(this IDataRecord dr, string name)
        {
            return ToNullableLong(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable long.
        /// </summary>
        public static long? CastNullableLong(this IDataRecord dr, string name)
        {
            return ToNullableLong(dr[name]);
        }

        private static long? ToNullableLong(object cell)
        {
            return cell == DBNull.Value ? default(long?) : long.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to int.
        /// </summary>
        public static int CastInt(this IDataRecord dr, string name)
        {
            return ToNullableInt(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable int.
        /// </summary>
        public static int? CastNullableInt(this IDataRecord dr, string name)
        {
            return ToNullableInt(dr[name]);
        }

        private static int? ToNullableInt(object cell)
        {
            return cell == DBNull.Value ? default(int?) : int.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to short.
        /// </summary>
        public static short CastShort(this IDataRecord dr, string name)
        {
            return ToNullableShort(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable short.
        /// </summary>
        public static short? CastNullableShort(this IDataRecord dr, string name)
        {
            return ToNullableShort(dr[name]);
        }

        private static short? ToNullableShort(object cell)
        {
            return cell == DBNull.Value ? default(short?) : short.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to byte.
        /// </summary>
        public static byte CastByte(this IDataRecord dr, string name)
        {
            return ToNullableByte(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable byte.
        /// </summary>
        public static byte? CastNullableByte(this IDataRecord dr, string name)
        {
            return ToNullableByte(dr[name]);
        }

        private static byte? ToNullableByte(object cell)
        {
            return cell == DBNull.Value ? default(byte?) : byte.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to double.
        /// </summary>
        public static double CastDouble(this IDataRecord dr, string name)
        {
            return ToNullableDouble(dr[name]) ?? double.NaN;
        }

        /// <summary>
        /// Cast cell to nullable double.
        /// </summary>
        public static double? CastNullableDouble(this IDataRecord dr, string name)
        {
            return ToNullableDouble(dr[name]);
        }

        private static double? ToNullableDouble(object cell)
        {
            return cell == DBNull.Value ? default(double?) : double.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to decimal.
        /// </summary>
        public static decimal CastDecimal(this IDataRecord dr, string name)
        {
            return ToNullableDecimal(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable decimal.
        /// </summary>
        public static decimal? CastNullableDecimal(this IDataRecord dr, string name)
        {
            return ToNullableDecimal(dr[name]);
        }

        private static decimal? ToNullableDecimal(object cell)
        {
            return cell == DBNull.Value ? default(decimal?) : decimal.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to string.
        /// </summary>
        public static string CastString(this IDataRecord dr, string name)
        {
            return ToString(dr[name]);
        }

        private static string ToString(object cell)
        {
            return cell == DBNull.Value ? default : cell.ToString();
        }


        /// <summary>
        /// Cast cell to char.
        /// </summary>
        public static char CastChar(this IDataRecord dr, string name)
        {
            return ToNullableChar(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable char.
        /// </summary>
        public static char? CastNullableChar(this IDataRecord dr, string name)
        {
            return ToNullableChar(dr[name]);
        }

        private static char? ToNullableChar(object cell)
        {
            return cell == DBNull.Value ? default(char?) : char.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to DateTimeOffset.
        /// </summary>
        public static DateTimeOffset CastDateTimeOffset(this IDataRecord dr, string name)
        {
            return ToNullableDateTimeOffset(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable DateTimeOffset.
        /// </summary>
        public static DateTimeOffset? CastNullableDateTimeOffset(this IDataRecord dr, string name)
        {
            return ToNullableDateTimeOffset(dr[name]);
        }

        private static DateTimeOffset? ToNullableDateTimeOffset(object cell)
        {
            return cell == DBNull.Value ? default(DateTimeOffset?) : DateTimeOffset.Parse(cell.ToString());
        }


        /// <summary>
        /// Cast cell to bool.
        /// </summary>
        public static bool CastBool(this IDataRecord dr, string name)
        {
            return ToNullableBool(dr[name]) ?? default;
        }

        /// <summary>
        /// Cast cell to nullable bool.
        /// </summary>
        public static bool? CastNullableBool(this IDataRecord dr, string name)
        {
            return ToNullableBool(dr[name]);
        }

        private static bool? ToNullableBool(object cell)
        {
            if (cell == DBNull.Value)
                return default;
            if (bool.TryParse(cell.ToString(), out bool o))
                return o;
            return (cell.ToString()) switch
            {
                "0" => false,
                "1" => true,
                _ => throw new FormatException($"Cell value '{cell}' was not recognized as a valid {typeof(bool).FullName}."),
            };
        }


        /// <summary>
        /// Cast cell to <seealso cref="Uri"/>.
        /// </summary>
        public static Uri CastUri(this IDataRecord dr, string name, UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            Uri.TryCreate(ToString(dr[name]), uriKind, out Uri uri);
            return uri;
        }
    }
}
