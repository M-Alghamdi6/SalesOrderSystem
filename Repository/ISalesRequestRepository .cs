using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Repository
{
    public interface ISalesRequestRepository
    {
        Task<JSONResponseDTO<IEnumerable<SalesRequestModel>>> GetPendingRequestsByUser(string username);
    }
}
