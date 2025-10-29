using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Services;
using System.Data;
using System.Net;

namespace SalesOrderSystem_BackEnd.Repository
{
    public class SalesRequestLineRepository : RepositoryBase<SalesRequestLineModel>,ISalesRequestLineRepository
    {
        public SalesRequestLineRepository(IDbConnection sqlConnection, IMappingService mapping)
            : base(sqlConnection, mapping, "SalesRequestLine")
        {
        }

        // Get all lines for a specific request
        public async Task<JSONResponseDTO<IEnumerable<SalesRequestLineModel>>> GetByRequestIdAsync(int requestId)
        {
            try
            {
                var query = new SqlKata.Query(_tableName).Where("SalesRequestId", requestId).Select("*");
                var sqlResult = _compiler.Compile(query);

                var data = await _sqlConnection.QueryAsync<SalesRequestLineModel>(sqlResult.Sql, sqlResult.NamedBindings);

                return new JSONResponseDTO<IEnumerable<SalesRequestLineModel>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = data,
                    Message = "Lines retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<IEnumerable<SalesRequestLineModel>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error retrieving lines: {ex.Message}"
                };
            }
        }
    }
}
