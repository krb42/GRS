using GRS.Data.Model.Repositories.Utilities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GRS.Data.Model.Extensions
{
   public static class SqlDataReaderPopulateExtensions
   {
      private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private static APropertyValue DetermineNewValue<T>(this PropertyInfo prop, object obj, object value)
      {
         try
         {
            var propType = prop.PropertyType;
            var isNullable = (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>));
            var result = new APropertyValue() { IsModified = false, Value = null, };

            var newValue = value;
            object oldValue = null;

            if (prop.GetValue(obj, null) != null)
            {
               oldValue = Cast.To<T>(prop.GetValue(obj, null));
            }
            if (isNullable && oldValue == null && (newValue == null || newValue.Equals(default(T))))
            {
               newValue = null;
            }
            if (!isNullable && newValue == null)
            {
               newValue = default(T);
            }

            if (oldValue == null && newValue == null)
            {
               // No modification required to the result.Value
            }
            else if ((oldValue == null && newValue != null) || (oldValue != null && newValue == null) || (!oldValue.Equals(newValue)))
            {
               result.IsModified = true;
               result.Value = newValue;
            }
            return result;
         }
         catch (Exception ex)
         {
            var method = $"{MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name}<T=={typeof(T).Name}>";
            log.Error($"{method} - target = {obj.GetType().Name}, propertyName = {prop.Name}, value = {value}");

            ex.Data[$"Error occurred during execution of SqlDataReader.Populate.{method}"] = string.Empty;
            ex.Data["Target"] = obj.GetType().Name;
            ex.Data["Property Name"] = prop.Name;
            ex.Data["Value"] = value;
            throw;
         }
      }

      private static DbColumnOptionsAttribute GetDbColumnOptionsAttribute(this PropertyInfo prop)
      {
         try
         {
            var attrib = (DbColumnOptionsAttribute)prop.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(DbColumnOptionsAttribute));
            if (attrib != null)
            {
               return attrib;
            }
         }
         catch (Exception ex)
         {
            var method = $"{MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name}";
            ex.Data[$"Error occurred during execution of SqlDataReader.Populate.{method}"] = string.Empty;
            ex.Data["Property Name"] = prop.Name;
            throw;
         }

         return null;
      }

      private static bool SetPropertyValue<T>(this PropertyInfo prop, object obj, object value)
      {
         try
         {
            var result = DetermineNewValue<T>(prop, obj, value);

            if (result.IsModified)
            {
               prop.SetValue(obj, Cast.To<T>(result.Value), null);
            }
            return result.IsModified;
         }
         catch (Exception ex)
         {
            var method = $"{MethodBase.GetCurrentMethod().DeclaringType.Name}.{MethodBase.GetCurrentMethod().Name}<T=={typeof(T).Name}>";
            log.Error($"{method} - target = {obj.GetType().Name}, propertyName = {prop.Name}, value = {value}");

            ex.Data[$"Error occurred during execution of SqlDataReader.Populate.{method}"] = string.Empty;
            ex.Data["Target"] = obj.GetType().Name;
            ex.Data["Property Name"] = prop.Name;
            ex.Data["Value"] = value;
            throw;
         }
      }

      private class APropertyValue
      {
         public bool IsModified { get; set; }

         public object Value { get; set; }
      }

      public static bool DBColumnIsImmutable(this PropertyInfo prop) => prop.GetDbColumnOptionsAttribute()?.IsImmutable ?? false;

      public static bool DBColumnIsOptional(this PropertyInfo prop) => prop.GetDbColumnOptionsAttribute()?.IsOptional ?? false;

      public static string DBColumnName(this PropertyInfo prop) => prop.Name;

      public static bool HasColumn(this SqlDataReader reader, string columnName)
      {
         foreach (DataRow row in reader.GetSchemaTable().Rows)
         {
            if (row["ColumnName"].ToString() == columnName)
               return true;
         } //Still here? Column not found.
         return false;
      }

      public static T Populate<T>(this SqlDataReader reader, bool throwExceptionIfColumnNotFound = true)
      {
         var sbErrors = new StringBuilder();
         var tType = typeof(T);

         var obj = Activator.CreateInstance(tType);

         foreach (var prop in tType.GetProperties())
         {
            var propInfo = tType.GetProperty(prop.Name);
            var propType = propInfo.PropertyType;

            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
               propType = propType.GetGenericArguments()[0];
            }

            var dbFieldName = prop.DBColumnName();
            if (string.IsNullOrEmpty(dbFieldName))
            {
               // DbFieldName attribute could not be found
               continue;
            }

            // Check to see if the reader has the DbFieldName
            if (!reader.HasColumn(prop.Name))
            {
               // DbFieldName is not found in gthe SqlDataReader
               if (throwExceptionIfColumnNotFound && !prop.DBColumnIsOptional())
               {
                  throw new KeyNotFoundException($"Column '{dbFieldName}' not found in SqlDataReader");
               }
               continue;
            }

            // We have the column and the DB Field Name

            if (propType == typeof(String))
            {
               prop.SetPropertyValue<string>(obj, reader.GetNullableString(dbFieldName));
            }
            else if (propType == typeof(Boolean))
            {
               prop.SetPropertyValue<bool>(obj, reader.GetNullableBoolean(dbFieldName));
            }
            else if (propType == typeof(DateTime))
            {
               prop.SetPropertyValue<DateTime>(obj, reader.GetNullableDateTime(dbFieldName));
            }
            else if (propType == typeof(Int32))
            {
               prop.SetPropertyValue<Int32>(obj, reader.GetNullableInt32(dbFieldName));
            }
            else if (propType == typeof(Int64))
            {
               prop.SetPropertyValue<Int64>(obj, reader.GetNullableInt64(dbFieldName));
            }
         }

         return (T)obj;
      }
   }
}
