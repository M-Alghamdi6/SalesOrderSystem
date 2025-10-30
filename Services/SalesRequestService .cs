using Dapper;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Repository;
using System.Data;
using System.Net;

namespace SalesOrderSystem_BackEnd.Services;

public class SalesRequestService : RepositoryBase<SalesRequestModel>, ISalesRequestService
{
   

    public SalesRequestService(IDbConnection sqlConnection, IMappingService mapping)
        : base(sqlConnection, mapping, "SalesRequest")
    {
        
    }


    private async Task<string> GenerateSalesRequestNo()
    {
        var allRequests = await this.GetAllAsync();
        var count = allRequests.Data?.Count() ?? 0;
        return $"SR-";
    }


    private async Task<IEnumerable<UsersModel>> GetApprovers()
    {
        var query = new SqlKata.Query("apps.Users").Where("Role", "Approver");
        var sqlResult = _compiler.Compile(query);
        return await _sqlConnection.QueryAsync<UsersModel>(sqlResult.Sql, sqlResult.NamedBindings);
    }

 
    private async Task<string> GetAutoApprover()
    {
        var approvers = await GetApprovers();
        return approvers.FirstOrDefault()?.UserName ?? "NoApproverFound";
    }

    private string GetStatus(string approver)
    {
        return string.IsNullOrEmpty(approver) ? "Draft" : "Pending";
    }

    private async Task<string> GetReasonFromApproverApi(string approver)
    {
        await Task.Delay(50); 
        return $"Reason provided by {approver}";
    }

    // ------------------- Create -------------------
    public async Task<JSONResponseDTO<SalesRequesterTableDTO>> CreateSalesRequest(SalesRequestModel model)
    {
        model.SalesRequestNo = await GenerateSalesRequestNo();
        model.Approver = await GetAutoApprover();
        model.Status = GetStatus(model.Approver);
        model.Reason = await GetReasonFromApproverApi(model.Approver);

        var result = await this.AddAsync(model);

       
        return _mapping.MapResponse<SalesRequestModel, SalesRequesterTableDTO>(result);
    }

    // ------------------- Get All -------------------
    public async Task<JSONResponseDTO<IEnumerable<SalesRequesterTableDTO>>> GetAllSalesRequests()
    {
        var result = await this.GetAllAsync();
        return _mapping.MapResponse<IEnumerable<SalesRequestModel>, IEnumerable<SalesRequesterTableDTO>>(result);
    }

    // ------------------- Get By Id -------------------
    public async Task<JSONResponseDTO<SalesRequesterTableDTO?>> GetSalesRequestById(int id)
    {
        var result = await this.GetByIdAsync(id);
        return _mapping.MapResponse<SalesRequestModel, SalesRequesterTableDTO>(result);
    }

    // ------------------- Update -------------------
    public async Task<JSONResponseDTO<SalesRequesterTableDTO>> UpdateSalesRequest(int id, SalesRequesterTableDTO dto)
    {
        var existing = await this.GetByIdAsync(id);
        if (existing.Data == null)
            return new JSONResponseDTO<SalesRequesterTableDTO>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Sales Request not found"
            };

       
        if (!DateTime.TryParse(dto.SalesDate, out var parsedDate))
        {
            return new JSONResponseDTO<SalesRequesterTableDTO>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Invalid SalesDate format"
            };
        }

     
        existing.Data.SalesDate = parsedDate;
        existing.Data.SalesNote = dto.SalesNote;
        

        var updateResult = await this.UpdateAsync(id, existing.Data);
        return _mapping.MapResponse<SalesRequestModel, SalesRequesterTableDTO>(updateResult);
    }


    // ------------------- Delete -------------------
    public async Task<JSONResponseDTO<object>> DeleteSalesRequest(int id)
    {
        var request = await this.GetByIdAsync(id);
        if (request.Data == null)
            return new JSONResponseDTO<object>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Sales Request not found",
                Id = id
            };

        if (request.Data.Status == "Approved" || request.Data.Status == "Rejected")
            return new JSONResponseDTO<object>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Cannot delete approved/rejected request",
                Id = id
            };

        return await this.DeleteAsync(id);
    }

    // ------------------- Cancel -------------------
    public async Task<JSONResponseDTO<SalesRequesterTableDTO>> CancelSalesRequest(int id)
    {
        var request = await this.GetByIdAsync(id);
        if (request.Data == null)
            return new JSONResponseDTO<SalesRequesterTableDTO>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Sales Request not found"
            };

        if (request.Data.Status != "Approved" && request.Data.Status != "Rejected")
            return new JSONResponseDTO<SalesRequesterTableDTO>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Only approved or rejected requests can be cancelled"
            };

        request.Data.Status = "Cancelled";
        var result = await this.UpdateAsync(id, request.Data);

        return _mapping.MapResponse<SalesRequestModel, SalesRequesterTableDTO>(result);
    }
    public async Task<ServiceResponse<IEnumerable<SalesRequestModel>>> GetSalesRequestsByUserId(int userId)
    {
        var response = new ServiceResponse<IEnumerable<SalesRequestModel>>();

        var query = "SELECT * FROM [dbo].[SalesRequest] WHERE UserId = @UserId";

        var requests = await _sqlConnection.QueryAsync<SalesRequestModel>(query, new { UserId = userId });

        response.Data = requests;
        response.StatusCode = System.Net.HttpStatusCode.OK;
        return response;
    }
}
