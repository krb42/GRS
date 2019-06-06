using GRS.Data.Model.Extensions;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GRS.Data.Model.Repositories.Utilities
{
    internal class RepositoryHelper : IRepositoryHelper
    {
        private readonly IBaseRepository _baseRepository;

        private DataTable CreateAndFillTable(SqlCommand selectCommand)
        {
            using (var adapter = new SqlDataAdapter(selectCommand))
            {
                var result = new DataTable();
                result.BeginLoadData();
                adapter.Fill(result);
                result.EndLoadData();
                return result;
            }
        }

        private int ExecuteNonQuery_Internal(SqlCommand command)
        {
            foreach (SqlParameter parameter in command.Parameters) if (parameter.Value == null) parameter.Value = DBNull.Value;
            return command.ExecuteNonQuery();
        }

        protected readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IEnumerable<SqlParameter> ConvertToParameters<T>(params T[] values)
        {
            return Enumerable.Range(1, values.Length)
               .Select(i => new SqlParameter($"@Item{i}", values[i - 1]));
        }

        public RepositoryHelper(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        private string ConnectionString => _baseRepository.ConnectionString;

        public SqlCommand BuildUpdateCommand<T>(T source, T original)
        {
            var sqlParams = new List<SqlParameter>();
            var sqlUpdates = new List<string>();
            var tType = typeof(T);

            foreach (var prop in tType.GetProperties())
            {
                var propInfo = tType.GetProperty(prop.Name);
                var propType = propInfo.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = propType.GetGenericArguments()[0];
                }

                var dbFieldName = prop.DBColumnName();
                if (string.IsNullOrEmpty(dbFieldName) || prop.DBColumnIsImmutable())
                {
                    // DbFieldName attribute could not be found, therefore not available to be update
                    // or is not allowed to be updated (i.e. immutable)
                    continue;
                }

                sqlUpdates.Add($"{dbFieldName} = @Value{dbFieldName}");
                sqlParams.Add(new SqlParameter($"@Value{dbFieldName}", prop.GetValue(source)));
            }

            // Add the Modification Update Information
            sqlUpdates.Add($"[TSModifyDate] = @TSModifyDate");
            sqlUpdates.Add("[TSModifyUser] = @TSModifyUser");
            sqlUpdates.Add("[VersionAutoID] = NEXT VALUE FOR [dbo].[VersionAutoID_Sequence]");

            sqlParams.Add(new SqlParameter($"@TSModifyDate", DateTime.Now));
            sqlParams.Add(new SqlParameter($"@TSModifyUser", _baseRepository.CurrentUserName));

            // Build command
            var cmd = new SqlCommand(string.Join(", ", sqlUpdates));
            cmd.Parameters.AddRange(sqlParams.ToArray());

            return cmd;
        }

        public SqlConnection CreateConnection() => new SqlConnection(ConnectionString);

        public int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return ExecuteNonQuery(conn, commandText, parameters);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute non query", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public int ExecuteNonQuery(SqlConnection connection, string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var comm = new SqlCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                        comm.Parameters.AddRange(parameters);
                    }
                    return ExecuteNonQuery_Internal(comm);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute non query", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            if (command.Connection == null)
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    return ExecuteNonQuery_Internal(command);
                }
            }
            else
                return ExecuteNonQuery_Internal(command);
        }

        public object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return ExecuteScalar(conn, commandText, parameters);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute scalar query", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public object ExecuteScalar(SqlConnection conn, string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var comm = new SqlCommand(commandText, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                        comm.Parameters.AddRange(parameters);
                    }
                    return ExecuteScalar(comm);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute scalar query", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public object ExecuteScalar(SqlCommand command) => command.ExecuteScalar();

        public void ForEach(string commandText, Action<SqlDataReader> mapFunction, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                            comm.Parameters.AddRange(parameters);
                        }
                        conn.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mapFunction(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute ForEach", e);
                throw;
            }
        }

        public DataTable GetDataTable(string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return GetDataTable(conn, commandText, parameters);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to get data table", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public DataTable GetDataTable(SqlConnection conn, string commandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var comm = new SqlCommand(commandText, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                        comm.Parameters.AddRange(parameters);
                    }
                    return GetDataTable(comm);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to get data table", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public DataTable GetDataTable(SqlCommand command)
        {
            return CreateAndFillTable(command);
        }

        public IEnumerable<T> GetList<T>(string commandText, params SqlParameter[] parameters) => GetList(commandText, r => (T)r[0], parameters);

        public IEnumerable<T> GetList<T>(string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return GetList(conn, commandText, mapFunction, parameters);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute GetList", e);
                throw;
            }
        }

        public IEnumerable<T> GetList<T>(SqlConnection connection, string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {
            try
            {
                using (var comm = new SqlCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                        comm.Parameters.AddRange(parameters);
                    }

                    return GetList(comm, mapFunction);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute GetList", e);
                throw;
            }
        }

        public IEnumerable<T> GetList<T>(SqlCommand command, Func<SqlDataReader, T> mapFunction)
        {
            var resultList = new List<T>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var result = mapFunction(reader);
                    if (result != null)
                    {
                        resultList.Add(result);
                    }
                }
            }

            return resultList;
        }

        public T GetSingle<T>(string commandText, params SqlParameter[] parameters) => GetSingle(commandText, r => (T)r[0], parameters);

        public T GetSingle<T>(string commandText, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return GetSingle<T>(conn, commandText, mapFunction, parameters);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute GetSingle", e);
                throw; // using "throw" by itself will preserve the stack trace
            }
        }

        public T GetSingle<T>(SqlConnection connection, string commandText, Func<SqlDataReader, T> mapFunction, SqlParameter[] parameters)
        {
            try
            {
                using (var comm = new SqlCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters) if (param.Value == null) param.SqlValue = DBNull.Value;
                        comm.Parameters.AddRange(parameters);
                    }

                    return GetSingle(comm, mapFunction);
                }
            }
            catch (SqlException e)
            {
                log.Error("Failed to execute GetSingle", e);
                throw;
            }
        }

        public T GetSingle<T>(SqlCommand command, Func<SqlDataReader, T> mapFunction)
        {
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read()) return mapFunction(reader);
                return default(T);
            }
        }

        public T GetValueSpecific<T>(SqlDataReader reader, string columnName, T defaultIfNull)
        {
            var columnIndex = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(columnIndex)) return defaultIfNull;
            return (T)reader.GetValue(columnIndex);
        }

        public T GetValueSpecific<T>(SqlDataReader reader, string columnName)
        {
            var columnIndex = reader.GetOrdinal(columnName);
            return (T)reader.GetValue(columnIndex);
        }
    }
}
