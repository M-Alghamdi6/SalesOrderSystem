using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrderSystem_BackEnd.Models
{
    public class SalesRequestLineModel
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(SalesRequest))]
        public int SalesRequestId { get; set; }

        public int LineNo { get; set; }

        [Required, StringLength(50)]
        public string ItemCode { get; set; } = "";

        [StringLength(200)]
        public string ItemDescription { get; set; } = "";

        [StringLength(20)]
        public string ItemUnit { get; set; } = "";

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Amount { get; set; }

        
        public virtual required SalesRequestModel SalesRequest { get; set; }
    }
}
