using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Repository
{
    public interface IUsersRepository : IRepositoryBase<UsersModel>
    {
        Task<JSONResponseDTO<UsersModel?>> AuthenticateAsync(UserDTO user);
        Task<IEnumerable<UsersModel>> GetUsersByRole(string role);
  }
}
