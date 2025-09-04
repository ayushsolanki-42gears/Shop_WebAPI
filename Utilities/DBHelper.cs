using System.Data;
using Microsoft.Data.SqlClient;

namespace MyWebApiApp.Utilities
{
    public class DBHelper
    {
        private readonly string _connectionString;

        public DBHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        // create and open sql connection (sync)
        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        // create and open sql connection (async)
        public async Task<SqlConnection> GetConnectionAsync()
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        // create cmd for given conn and stored procedure and add parameter list
        public SqlCommand CreateCommand(SqlConnection conn, string storedProcedureName, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }

        // -------------------------
        // Synchronous methods
        // -------------------------
        public DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = GetConnection();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public object ExecuteScalar(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = GetConnection();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            return cmd.ExecuteScalar();
        }

        public int ExecuteNonQuery(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = GetConnection();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            return cmd.ExecuteNonQuery();
        }

        // -------------------------
        // Asynchronous methods
        // -------------------------
        public async Task<DataTable> ExecuteDataTableAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = await GetConnectionAsync();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            using var reader = await cmd.ExecuteReaderAsync();

            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

        public async Task<object?> ExecuteScalarAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = await GetConnectionAsync();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            using var conn = await GetConnectionAsync();
            using var cmd = CreateCommand(conn, storedProcedureName, parameters);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
