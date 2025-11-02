namespace SalesOrderSystem_BackEnd.DTOs
{
  public class SalesRequestLineDTO
  {
    public int? LineNumber { get; set; }
    public string ItemCode { get; set; } = "";
    public string ItemDescription { get; set; } = "";
    public string ItemUnit { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal? Amount { get; set; }
  }
}
