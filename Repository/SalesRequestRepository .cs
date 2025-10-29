using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Services;
using System.Data;

namespace SalesOrderSystem_BackEnd.Repository
{
    // Repository مسؤول عن التعامل المباشر مع قاعدة البيانات
    public class SalesRequestRepository : RepositoryBase<SalesRequestModel>, ISalesRequestRepository
    {
        public SalesRequestRepository(IDbConnection sqlConnection, IMappingService mapping)
            : base(sqlConnection, mapping, "SalesRequest")
        {
        }

        // مثال لدالة خاصة: جلب الطلبات Pending لمستخدم معين
        public async Task<JSONResponseDTO<IEnumerable<SalesRequestModel>>> GetPendingRequestsByUser(string username)
        {
            try
            {
                var query = new SqlKata.Query(_tableName)
                    .Where("RequesterUsername", username)
                    .Where("Status", "Pending");

                var sqlResult = _compiler.Compile(query);
                var data = await _sqlConnection.QueryAsync<SalesRequestModel>(sqlResult.Sql, sqlResult.NamedBindings);

                return new JSONResponseDTO<IEnumerable<SalesRequestModel>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = data,
                    Message = "Pending requests retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<IEnumerable<SalesRequestModel>>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error retrieving pending requests: {ex.Message}"
                };
            }
        }
    }
}
