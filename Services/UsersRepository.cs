using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Repository;
using System.Data;
using System.Net;

namespace SalesOrderSystem_BackEnd.Services
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
                var sql = "SELECT TOP 1 * FROM Users WHERE UserName = @UserName";
                var foundUser = await _sqlConnection.QueryFirstOrDefaultAsync<UsersModel>(sql, new { user.UserName });

                // 2️⃣ Check if user exists and password matches
                if (foundUser == null)
                {
                    return new JSONResponseDTO<UsersModel?>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                        Message = "Invalid username or password"
                    };
                }

                if (foundUser.Password != user.Password)
                {
                    return new JSONResponseDTO<UsersModel?>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                        Message = "Invalid username or password"
                    };
                }

                // 3️⃣ If success
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

    }
}
