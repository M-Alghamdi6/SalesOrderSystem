using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;

namespace SalesOrderSystem_BackEnd.Services
{
    public interface ISalesRequestLineService
    {
        Task<JSONResponseDTO<SalesRequestLineModel>> CreateLineAsync(SalesRequestLineModel line);
        Task<JSONResponseDTO<IEnumerable<SalesRequestLineModel>>> GetLinesByRequestAsync(int requestId);
        Task<JSONResponseDTO<SalesRequestLineModel?>> GetLineAsync(int id);
        Task<JSONResponseDTO<SalesRequestLineModel>> UpdateLineAsync(int id, SalesRequestLineModel line);
        Task<JSONResponseDTO<object>> DeleteLineAsync(int id);
    }
}
