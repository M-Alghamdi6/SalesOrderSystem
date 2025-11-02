namespace SalesOrderSystem_BackEnd.DTOs
{
  public class SalesRequestDTO
  {
    public int Id { get; set; }
    public string SalesRequestNo { get; set; } = string.Empty;
    public DateTime SalesDate { get; set; }
    public string SalesNote { get; set; } = string.Empty;
    public string Approver { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? RejectionRemark { get; set; }
    public string? ApprovalRemark { get; set; }
    public string RequesterUsername { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public int UserId { get; set; }

    public IEnumerable<SalesRequestLineDTO>? SalesRequestItems { get; set; }
  }
}
