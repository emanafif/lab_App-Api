using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_App.Models
{

    public class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LicenseKey { get; set; }

        [Required]
        public string UserId { get; set; }  

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime ExpirationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
