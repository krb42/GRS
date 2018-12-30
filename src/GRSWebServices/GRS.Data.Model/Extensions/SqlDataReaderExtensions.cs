using System;
using System.Data.SqlClient;

namespace GRS.Data.Model.Extensions
{
   //[DebuggerStepThrough]
   public static class SqlDataReaderExtensions
   {
      public static bool GetBoolean(this SqlDataReader reader, string columnName)
      {
         var index = reader.GetOrdinal(columnName);
         var dbType = reader.GetDataTypeName(index).ToUpper();

         switch (dbType)
         {
            case "TINYINT": return reader.GetByte(index) != 0;
            case "BIT": return reader.GetBoolean(index);
            case "INT": return reader.GetInt32(index) != 0;
            default: throw new ArgumentException($"Can't convert datatype {dbType} to boolean");
         }
      }

      public static bool GetBoolean(this SqlDataReader reader, string columnName, bool defaultIfNull) => reader.GetNullableBoolean(columnName) ?? defaultIfNull;

      public static byte GetByte(this SqlDataReader reader, string columnName) => reader.GetByte(reader.GetOrdinal(columnName));

      public static byte GetByte(this SqlDataReader reader, string columnName, byte defaultIfNull) => reader.GetNullableByte(columnName) ?? defaultIfNull;

      public static DateTime GetDate(this SqlDataReader reader, string columnName) => reader.GetDateTime(reader.GetOrdinal(columnName)).Date;

      public static DateTime GetDate(this SqlDataReader reader, string columnName, DateTime defaultIfNull) => reader.GetDateTime(columnName, defaultIfNull).Date;

      public static DateTime GetDateTime(this SqlDataReader reader, string columnName) => reader.GetDateTime(reader.GetOrdinal(columnName));

      public static DateTime GetDateTime(this SqlDataReader reader, string columnName, DateTime defaultIfNull) => reader.GetNullableDateTime(columnName) ?? defaultIfNull;

      public static decimal GetDecimal(this SqlDataReader reader, string columnName) => reader.GetDecimal(reader.GetOrdinal(columnName));

      public static decimal GetDecimal(this SqlDataReader reader, string columnName, decimal defaultIfNull) => reader.GetNullableDecimal(columnName) ?? defaultIfNull;

      public static short GetInt16(this SqlDataReader reader, string columnName) => reader.GetInt16(reader.GetOrdinal(columnName));

      public static short GetInt16(this SqlDataReader reader, string columnName, short defaultIfNull) => reader.GetNullableInt16(columnName) ?? defaultIfNull;

      public static int GetInt32(this SqlDataReader reader, string columnName) => reader.GetInt32(reader.GetOrdinal(columnName));

      public static int GetInt32(this SqlDataReader reader, string columnName, int defaultIfNull) => reader.GetNullableInt32(columnName) ?? defaultIfNull;

      public static long GetInt64(this SqlDataReader reader, string columnName) => reader.GetInt64(reader.GetOrdinal(columnName));

      public static long GetInt64(this SqlDataReader reader, string columnName, long defaultIfNull) => reader.GetNullableInt64(columnName) ?? defaultIfNull;

      public static bool? GetNullableBoolean(this SqlDataReader reader, string columnName)
      {
         var index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;

         return reader.GetBoolean(columnName);
      }

      public static byte? GetNullableByte(this SqlDataReader reader, string columnName)
      {
         var index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;

         return reader.GetByte(index);
      }

      public static DateTime? GetNullableDate(this SqlDataReader reader, string columnName) => reader.GetNullableDateTime(columnName)?.Date;

      public static DateTime? GetNullableDateTime(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetDateTime(index);
      }

      public static decimal? GetNullableDecimal(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetDecimal(index);
      }

      public static short? GetNullableInt16(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetInt16(index);
      }

      public static int? GetNullableInt32(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetInt32(index);
      }

      public static long? GetNullableInt64(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetInt64(index);
      }

      public static string GetNullableString(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetString(index);
      }

      public static TimeSpan? GetNullableTime(this SqlDataReader reader, string columnName)
      {
         int index = reader.GetOrdinal(columnName);
         if (reader.IsDBNull(index)) return null;
         return reader.GetTimeSpan(index);
      }

      public static string GetString(this SqlDataReader reader, string columnName) => reader.GetString(reader.GetOrdinal(columnName));

      public static string GetString(this SqlDataReader reader, string columnName, string defaultIfNull) => reader.GetNullableString(columnName) ?? defaultIfNull;
   }
}
