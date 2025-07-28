using System.ComponentModel.DataAnnotations;

namespace Lab_App.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public String username { get; set; }
        [Required]
        public String password { get; set; }
        [Required]
        public int labId { get; set; }
    }
}
