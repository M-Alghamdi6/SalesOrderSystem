using SalesOrderSystem_BackEnd.DTOs;
namespace SalesOrderSystem_BackEnd.Repository
{
    public interface IRepositoryBase<T>
    {
        Task<JSONResponseDTO<T>> AddAsync(T entity);
        Task<JSONResponseDTO<IEnumerable<T>>> GetAllAsync();
        Task<JSONResponseDTO<T?>> GetByIdAsync(int id);
        Task<JSONResponseDTO<object>> UpdateAsync(int id, T entity);
        Task<JSONResponseDTO<object>> DeleteAsync(int id);
    }
}
