using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Repository;
using System.Data;

namespace SalesOrderSystem_BackEnd.Services
{
    public class SalesRequestLineService : RepositoryBase<SalesRequestLineModel>, ISalesRequestLineService
    {
        public SalesRequestLineService(IDbConnection sqlConnection, IMappingService mapping)
            : base(sqlConnection, mapping, "SalesRequestLine")
        {
        }

        // Create a new line
        public Task<JSONResponseDTO<SalesRequestLineModel>> CreateLineAsync(SalesRequestLineModel line)
        {
            return AddAsync(line);
        }

        // Get all lines for a specific request
        public async Task<JSONResponseDTO<IEnumerable<SalesRequestLineModel>>> GetLinesByRequestAsync(int requestId)
        {
            try
            {
                var query = new SqlKata.Query(_tableName)
                                        .Where("SalesRequestId", requestId)
                                        .Select("*");
                var sqlResult = _compiler.Compile(query);

                var data = await _sqlConnection.QueryAsync<SalesRequestLineModel>(sqlResult.Sql, sqlResult.NamedBindings);

                return new JSONResponseDTO<IEnumerable<SalesRequestLineModel>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = data,
                    Message = "Lines retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<IEnumerable<SalesRequestLineModel>>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error retrieving lines: {ex.Message}"
                };
            }
        }

        // Get line by Id
        public Task<JSONResponseDTO<SalesRequestLineModel?>> GetLineAsync(int id)
        {
            return GetByIdAsync(id);
        }

        // Update line
        public Task<JSONResponseDTO<SalesRequestLineModel>> UpdateLineAsync(int id, SalesRequestLineModel line)
        {
            return UpdateAsync(id, line);
        }

        // Delete line
        public Task<JSONResponseDTO<object>> DeleteLineAsync(int id)
        {
            return DeleteAsync(id);
        }
    }
}
