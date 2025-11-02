using Microsoft.AspNetCore.Mvc;
using SalesOrderSystem_BackEnd.DTOs;
using SalesOrderSystem_BackEnd.Repository;
using Microsoft.Data.SqlClient;
using Dapper;

namespace SalesOrderSystem_BackEnd.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly SqlConnection _sqlConnection;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IUsersRepository _userService;
  public UsersController(
      SqlConnection sqlConnection,
      IHttpContextAccessor httpContextAccessor,
      IUsersRepository userService) // Inject here
  {
    _sqlConnection = sqlConnection;
    _httpContextAccessor = httpContextAccessor;
    _userService = userService; // Assign
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] UserDTO dto)
  {
    if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
      return BadRequest("Username and password are required");

    await _sqlConnection.OpenAsync();

    var user = await _sqlConnection.QueryFirstOrDefaultAsync<UserRecord>(
        "SELECT Username, Password, Role FROM [apps].[Users] WHERE Username = @Username",
        new { Username = dto.UserName });

    if (user == null || user.Password != dto.Password)
      return Unauthorized("Invalid username or password");

    _httpContextAccessor.HttpContext!.Session.SetString("Username", user.Username);
    _httpContextAccessor.HttpContext.Session.SetString("Role", user.Role);

    return Ok(new
    {
      message = "Login successful",
      username = user.Username,
      role = user.Role
    });
  }

  [HttpGet("me")]
  public IActionResult GetCurrentUser()
  {
    var username = _httpContextAccessor.HttpContext?.Session.GetString("Username");
    var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");

    if (username == null)
      return Unauthorized("No user logged in");

    return Ok(new { username, role });
  }

  [HttpGet("approver-only")]
  public IActionResult ApproverOnly()
  {
    var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");

    if (role != "Approver")
      return Forbid("Access denied — Approver only");

    return Ok("Welcome Approver! You can access this resource.");
  }

  [HttpGet("approvers")]
  public async Task<IActionResult> GetApprovers()
  {
    var approvers = await _userService.GetUsersByRole("Approver");
    return Ok(approvers.Select(a => new { a.Id, a.UserName }));
  }

  private class UserRecord
  {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
  }
}
