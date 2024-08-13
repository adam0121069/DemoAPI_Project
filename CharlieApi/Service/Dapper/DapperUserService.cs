using System.Data.SqlClient;
using CharlieApi.Models.DapperModels;
using Dapper;

namespace CharlieApi.DapperService
{
    public class DapperUserService
    {
        private readonly string _connectionString;

        public DapperUserService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<DapperUser>> GetUsersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                IEnumerable<DapperUser> users = await connection.QueryAsync<DapperUser>("SELECT * FROM Users");
                return users ?? Enumerable.Empty<DapperUser>();
            }
        }

        public async Task<DapperUser> GetUserByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                DapperUser? user = await connection.QueryFirstOrDefaultAsync<DapperUser>("SELECT * FROM Users WHERE SeqNo = @Id", new { Id = id });
                return user ?? new DapperUser();
            }
        }

        public async Task<int> UpdateUserAsync(DapperUser user)
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

        public async Task<int> CreateUserAsync(DapperUser user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = @"
                    INSERT INTO Users (AccountNumber, PassWordStr, FirstName, LastName, Email ,DateOfBirth)
                    VALUES (@AccountNumber, @PassWordStr, @FirstName, @LastName, @Email, @DateOfBirth);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                var result = await connection.QuerySingleAsync<int>(sql, new
                {
                    user.AccountNumber,
                    user.PassWordStr,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    // Convert DateOnly to DateTime
                    DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue)
                });

                return result;
            }
        }
    }
}
