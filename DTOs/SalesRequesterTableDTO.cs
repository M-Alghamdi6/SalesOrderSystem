namespace SalesOrderSystem_BackEnd.DTOs
{
  public class SalesRequesterTableDTO
  {
    public int Id { get; set; }                 // Unique identifier added
    public string SR { get; set; } = "";
    public string SalesDate { get; set; } = "";        // Consider using DateTime if appropriate
    public string SalesNote { get; set; } = "";
    public string Approver { get; set; } = "";
    public string Status { get; set; } = "";
    public string Reason { get; set; } = "";
    public string Actions { get; set; } = "";
  }
}
