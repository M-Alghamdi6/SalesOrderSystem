using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Repository
{
    public interface ISalesRequestLineRepository
    {
        Task<JSONResponseDTO<IEnumerable<SalesRequestLineModel>>> GetByRequestIdAsync(int requestId);
    }
}
