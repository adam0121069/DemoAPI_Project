using System.Data.SqlClient;
using CharlieApi.Models.DapperModels;
using Dapper;

namespace CharlieApi.DapperService
{
    public class DapperUser
    {
        private readonly string _connectionString;

        public DapperUser(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Models.DapperModels.DapperUser>> GetUsersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                IEnumerable<Models.DapperModels.DapperUser> users = await connection.QueryAsync<Models.DapperModels.DapperUser>("SELECT * FROM Users");
                return users ?? Enumerable.Empty<Models.DapperModels.DapperUser>();
            }
        }

        public async Task<Models.DapperModels.DapperUser> GetUserByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                Models.DapperModels.DapperUser? user = await connection.QueryFirstOrDefaultAsync<Models.DapperModels.DapperUser>("SELECT * FROM Users WHERE SeqNo = @Id", new { Id = id });
                return user ?? new Models.DapperModels.DapperUser();
            }
        }

        public async Task<int> UpdateUserAsync(Models.DapperModels.DapperUser user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = @"
                UPDATE Users
                SET 
                    AccountNumber = @AccountNumber,
                    PassWordStr = @PassWordStr,
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email
                WHERE SeqNo = @SeqNo";

                var result = await connection.ExecuteAsync(sql, new
                {
                    user.AccountNumber,
                    user.PassWordStr,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.SeqNo
                });

                return result;
            }
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "DELETE FROM Users WHERE SeqNo = @Id";

                var result = await connection.ExecuteAsync(sql, new { Id = id });

                return result; // 返回受影响的行数
            }
        }
    }
}
