using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Services;


namespace SalesOrderSystem_BackEnd.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesRequesterController : ControllerBase
    {
        private readonly ISalesRequestService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SqlConnection _sqlConnection;

        public SalesRequesterController(ISalesRequestService service, IHttpContextAccessor httpContextAccessor, SqlConnection sqlConnection)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _sqlConnection = sqlConnection;
        }

        // Get all (for table)
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            StatusCode((int)(await _service.GetAllSalesRequests()).StatusCode,
                       await _service.GetAllSalesRequests());

        // Create new sales request

        [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalesRequestDTO model)
        {
            var username = _httpContextAccessor.HttpContext?.Session.GetString("Username");
            if (username == null)
                return Unauthorized("No user logged in");

            var user = await _sqlConnection.QueryFirstOrDefaultAsync<UserRecord>(
                "SELECT Id FROM [apps].[Users] WHERE Username = @Username",
                new { Username = username });

            if (user == null)
                return Unauthorized("User not found");

            // Map DTO to Model
            var salesRequest = new SalesRequestModel
            {
                SalesNote = model.SalesNote,
                SalesDate = DateTime.Now,
                UserId = user.Id,             // <-- FK reference
                RequesterUsername = username
            };

            var result = await _service.CreateSalesRequest(salesRequest);
            return Ok(result);
        }

        // Delete request
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteSalesRequest(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // Cancel request
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelSalesRequest(id);
            return StatusCode((int)result.StatusCode, result);
        }
        private class UserRecord
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }


}
