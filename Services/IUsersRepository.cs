using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Repository;

namespace SalesOrderSystem_BackEnd.Services
{
    public interface IUsersRepository : IRepositoryBase<UsersModel>
    {
        Task<JSONResponseDTO<UsersModel?>> AuthenticateAsync(UserDTO user);
    }
}
