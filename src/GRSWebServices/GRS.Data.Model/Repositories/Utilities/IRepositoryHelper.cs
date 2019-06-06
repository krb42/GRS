using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GRS.Data.Model.Repositories.Utilities
{
    public interface IRepositoryHelper
    {
        SqlCommand BuildUpdateCommand<T>(T source, T original);

        SqlConnection CreateConnection();

        int ExecuteNonQuery(SqlCommand command);

        int ExecuteNonQuery(string commandText, params SqlParameter[] parameters);

        int ExecuteNonQuery(SqlConnection connection, string commandText, params SqlParameter[] parameters);

        object ExecuteScalar(SqlCommand command);

        object ExecuteScalar(string commandText, params SqlParameter[] parameters);

        object ExecuteScalar(SqlConnection conn, string commandText, params SqlParameter[] parameters);

        void ForEach(string commandText, Action<SqlDataReader> mapFunction, params SqlParameter[] parameters);

        DataTable GetDataTable(string commandText, params SqlParameter[] parameters);

        DataTable GetDataTable(SqlConnection connection, string commandText, params SqlParameter[] parameters);

        DataTable GetDataTable(SqlCommand command);

        IEnumerable<T> GetList<T>(SqlCommand command, Func<SqlDataReader, T> mapFunction);

        IEnumerable<T> GetList<T>(string commandText, params SqlParameter[] parameters);

        IEnumerable<T> GetList<T>(string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters);

        IEnumerable<T> GetList<T>(SqlConnection connection, string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters);

        T GetSingle<T>(SqlCommand command, Func<SqlDataReader, T> mapFunction);

        T GetSingle<T>(string commandText, params SqlParameter[] parameters);

        T GetSingle<T>(string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters);

        T GetSingle<T>(SqlConnection connection, string commandText, Func<SqlDataReader, T> mapFunction, SqlParameter[] parameters);

        T GetValueSpecific<T>(SqlDataReader reader, string columnName);

        T GetValueSpecific<T>(SqlDataReader reader, string columnName, T defaultIfNull);
    }
}
