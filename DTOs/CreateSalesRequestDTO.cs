namespace SalesOrderSystem_BackEnd.DTOs
{
    public class CreateSalesRequestDTO
    {
        public string SalesRequestNo { get; set; } = "";
        public DateTime SalesDate { get; set; }
        public string SalesNote { get; set; } = "";
        public string Approver { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public string? RejectionRemark { get; set; }
        public string? ApprovalRemark { get; set; }
        public string RequesterUsername { get; set; } = "";
        public string? Reason { get; set; }
        public int UserId { get; set; }

    public IEnumerable<SalesRequestLineDTO>? SalesRequestItems { get; set; }
  }

}
