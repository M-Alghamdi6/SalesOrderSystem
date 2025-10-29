using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrderSystem_BackEnd.Models
{
    [Table("Users", Schema = "apps")]
    public class UsersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Role { get; set; } = "Requester"; // Requester or Approver

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IEnumerable<SalesRequestModel>? SalesRequests { get; set; }
    }
    
}
