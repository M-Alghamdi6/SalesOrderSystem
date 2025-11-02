using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Services;
using System.Data;
using System.Net;

namespace SalesOrderSystem_BackEnd.Repository
{
    public class UsersRepository : RepositoryBase<UsersModel>, IUsersRepository
    {
        public UsersRepository(IDbConnection sqlConnection, IMappingService mapping)
            : base(sqlConnection, mapping, "apps.Users")
        {
        }
        public async Task<JSONResponseDTO<UsersModel?>> AuthenticateAsync(UserDTO user)
        {
            try
            {
                // 1️⃣ Get user by username
                var sql = "SELECT TOP 1 * FROM [apps].[Users] WHERE UserName = @UserName"; ;
                var foundUser = await _sqlConnection.QueryFirstOrDefaultAsync<UsersModel>(
                    sql, new { UserName = user.UserName });

                // 2️⃣ Check if user exists and password matches
                if (foundUser == null || foundUser.Password != user.Password)
                {
                    return new JSONResponseDTO<UsersModel?>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                        Message = "Invalid username or password"
                    };
                }

                // 3️⃣ Login successful
                return new JSONResponseDTO<UsersModel?>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = foundUser,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<UsersModel?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Login error: {ex.Message}"
                };
            }
        }
    public async Task<IEnumerable<UsersModel>> GetUsersByRole(string role)
    {
      var sql = "SELECT * FROM [apps].[Users] WHERE Role = @Role";
      return await _sqlConnection.QueryAsync<UsersModel>(sql, new { Role = role });
    }

  }
}
