using Dapper;
using SqlKata;
using SqlKata.Compilers;
using System.Data;
using System.Net;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace SalesOrderSystem_BackEnd.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly IDbConnection _sqlConnection;
        protected readonly SqlServerCompiler _compiler;
        protected readonly string _tableName;
        protected readonly IMappingService _mapping;

        public RepositoryBase(IDbConnection sqlConnection, IMappingService mapping, string? tableName = null)
        {
            _sqlConnection = sqlConnection;
            _compiler = new SqlServerCompiler();
            _mapping = mapping;
            _tableName = tableName ?? typeof(T).Name.Replace("Model", "");
        }

        protected JSONResponseDTO<TDest> MapResponse<TSrc, TDest>(JSONResponseDTO<TSrc> source)
        {
            return _mapping.MapResponse<TSrc, TDest>(source);
        }

        public async Task<JSONResponseDTO<T>> AddAsync(T entity)
        {
            try
            {
                // Only include actual table columns (exclude navigation properties)
                var insertData = entity.GetType()
                                       .GetProperties()
                                       .Where(p => p.Name != "Id" &&
                                                   p.PropertyType.IsPrimitive ||
                                                   p.PropertyType == typeof(string) ||
                                                   p.PropertyType == typeof(DateTime) ||
                                                   Nullable.GetUnderlyingType(p.PropertyType) != null)
                                       .ToDictionary(p => p.Name, p => p.GetValue(entity));

                var query = new SqlKata.Query(_tableName).AsInsert(insertData);
                var sqlResult = _compiler.Compile(query);
                var sqlWithId = sqlResult.Sql + "; SELECT CAST(SCOPE_IDENTITY() as int);";

                var id = await _sqlConnection.ExecuteScalarAsync<int>(sqlWithId, sqlResult.NamedBindings);

                var idProperty = entity.GetType().GetProperty("Id");
                if (idProperty != null && idProperty.CanWrite)
                    idProperty.SetValue(entity, id);

                return new JSONResponseDTO<T>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = entity,
                    Message = "Record added successfully.",
                    Id = id
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<T>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error adding record: {ex.Message}"
                };
            }
        }

        public async Task<JSONResponseDTO<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var query = new SqlKata.Query(_tableName).Select("*");
                var sqlResult = _compiler.Compile(query);

                var data = await _sqlConnection.QueryAsync<T>(sqlResult.Sql, new Dictionary<string, object>());

                return new JSONResponseDTO<IEnumerable<T>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = data,
                    Message = "All records retrieved successfully."
                };
            }
            catch (System.Exception ex)
            {
                return new JSONResponseDTO<IEnumerable<T>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error retrieving records: {ex.Message}"
                };
            }
        }

        public async Task<JSONResponseDTO<T?>> GetByIdAsync(int id)
        {
            try
            {
        var query = new SqlKata.Query(_tableName).Where("Id", id);
        var sqlResult = _compiler.Compile(query);

        var parameters = new Dictionary<string, object> { ["p0"] = id };
        var data = await _sqlConnection.QueryFirstOrDefaultAsync<T>(sqlResult.Sql, sqlResult.NamedBindings);

        return new JSONResponseDTO<T?>
                {
                    StatusCode = data != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                    Data = data,
                    Message = data != null ? "Record found successfully." : "Record not found."
                };
            }
            catch (System.Exception ex)
            {
                return new JSONResponseDTO<T?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"Error retrieving record: {ex.Message}",
                    Id = id
                };
            }
        }

        public async Task<JSONResponseDTO<T>> UpdateAsync(int id, T entity)
        {
            try
            {
                var updateData = entity.GetType()
                                       .GetProperties()
                                       .Where(p => p.Name != "Id")
                                       .ToDictionary(p => p.Name, p => p.GetValue(entity));

                var query = new SqlKata.Query(_tableName)
                                .Where("Id", id)
                                .AsUpdate(updateData);

                var sqlResult = _compiler.Compile(query);

                var affected = await _sqlConnection.ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

                if (affected == 0)
                    return new JSONResponseDTO<T>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Record not found",
                        Data = null,
                        Id = id
                    };

                return new JSONResponseDTO<T>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Record updated successfully",
                    Data = entity,
                    Id = id
                };
            }
            catch (Exception ex)
            {
                return new JSONResponseDTO<T>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Error updating record: {ex.Message}",
                    Data = null,
                    Id = id
                };
            }
        }

        public async Task<JSONResponseDTO<object>> DeleteAsync(int id)
        {
            try
            {
        var query = new SqlKata.Query(_tableName).Where("Id", id).AsDelete();
        var sqlResult = _compiler.Compile(query);

        var parameters = new Dictionary<string, object> { ["p0"] = id };
        var affected = await _sqlConnection.ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

        return new JSONResponseDTO<object>
                {
                    StatusCode = affected > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                    Message = affected > 0 ? "Record deleted successfully." : "Record not found",
                    Data = null
                };
            }
            catch (System.Exception ex)
            {
                return new JSONResponseDTO<object>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Error deleting record: {ex.Message}",
                    Data = null,
                    Id = id
                };
            }
        }
    }
}
