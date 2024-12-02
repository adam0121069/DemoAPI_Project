using System.Data.SqlClient;
using CharlieApi.Models.DapperModels;
using Dapper;

namespace CharlieApi.DapperService
{
    public class DapperUserService : DapperBaseService
    {
        public DapperUserService(string connectionString) : base(connectionString) { }

        public Task<IEnumerable<DapperUser>> GetUsersAsync()
        {
            return QueryAsync<DapperUser>("SELECT * FROM Users");
        }

        public Task<DapperUser> GetUserByIdAsync(int id)
        {
            return QuerySingleOrDefaultAsync<DapperUser>("SELECT * FROM Users WHERE SeqNo = @Id", new { Id = id });
        }

        public Task<int> UpdateUserAsync(DapperUser user)
        {
            string sql = @"
                UPDATE Users
                SET 
                    AccountNumber = @AccountNumber,
                    PassWordStr = @PassWordStr,
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email
                WHERE SeqNo = @SeqNo";

            return ExecuteAsync(sql, new
            {
                user.AccountNumber,
                user.PassWordStr,
                user.FirstName,
                user.LastName,
                user.Email,
                user.SeqNo
            });
        }

        public Task<int> DeleteUserAsync(int id)
        {
            string sql = "DELETE FROM Users WHERE SeqNo = @Id";
            return ExecuteAsync(sql, new { Id = id });
        }

        public async Task<int> CreateUserAsync(DapperUser user)
        {
            string sql = @"
                INSERT INTO Users (AccountNumber, PassWordStr, FirstName, LastName, Email, DateOfBirth)
                VALUES (@AccountNumber, @PassWordStr, @FirstName, @LastName, @Email, @DateOfBirth);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            // 将 `DateOnly` 转为 `DateTime`
            var parameters = new
            {
                user.AccountNumber,
                user.PassWordStr,
                user.FirstName,
                user.LastName,
                user.Email,
                DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue)
            };

            return await ExecuteWithConnectionAsync(conn => conn.QuerySingleAsync<int>(sql, parameters));
        }
    }
}


