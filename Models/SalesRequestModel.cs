using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrderSystem_BackEnd.Models
{

public class SalesRequestModel
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string SalesRequestNo { get; set; } = "";

        [Required]
        public DateTime SalesDate { get; set; }

        [StringLength(200)]
        public string SalesNote { get; set; } = "";

        [StringLength(100)]
        public string Approver { get; set; } = "";

        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(200)]
        public string? RejectionRemark { get; set; }

        [StringLength(200)]
        public string? ApprovalRemark { get; set; }

        [Required, StringLength(100)]
        public string RequesterUsername { get; set; } = "";

        public string? Reason { get; set; }

        // 🔗 Foreign key relationship to User
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public UsersModel? User { get; set; } // Navigation property


        public IEnumerable<SalesRequestLineModel>? SalesRequestItems { get; set; }
    }
}
