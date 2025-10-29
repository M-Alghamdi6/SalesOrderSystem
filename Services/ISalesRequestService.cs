using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Repository;
using System.Net;


namespace SalesOrderSystem_BackEnd.Services
{
    public interface ISalesRequestService : IRepositoryBase<SalesRequestModel>
    {
        Task<JSONResponseDTO<SalesRequesterTableDTO>> CreateSalesRequest(SalesRequestModel model);
        Task<JSONResponseDTO<IEnumerable<SalesRequesterTableDTO>>> GetAllSalesRequests();
        Task<JSONResponseDTO<SalesRequesterTableDTO?>> GetSalesRequestById(int id);
        Task<JSONResponseDTO<SalesRequesterTableDTO>> UpdateSalesRequest(int id, SalesRequesterTableDTO dto);
        Task<JSONResponseDTO<object>> DeleteSalesRequest(int id);
        Task<JSONResponseDTO<SalesRequesterTableDTO>> CancelSalesRequest(int id);
        Task<ServiceResponse<IEnumerable<SalesRequestModel>>> GetSalesRequestsByUserId(int userId);
    }
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
    
}
