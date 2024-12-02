using System.Data.SqlClient;
using Dapper;

namespace CharlieApi.DapperService
{
    public abstract class DapperBaseService
    {
        protected readonly string _connectionString;

        protected DapperBaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<T> ExecuteWithConnectionAsync<T>(Func<SqlConnection, Task<T>> action)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await action(connection);
        }

        protected Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return ExecuteWithConnectionAsync(conn => conn.QueryAsync<T>(sql, parameters));
        }

        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null) where T : new()
        {
            var result = await ExecuteWithConnectionAsync(conn => conn.QuerySingleOrDefaultAsync<T>(sql, parameters));
            return result ?? new T(); // 如果结果为 null，返回 T 的默认实例
        }

        protected Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return ExecuteWithConnectionAsync(conn => conn.ExecuteAsync(sql, parameters));
        }
    }
}
