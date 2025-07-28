using System.ComponentModel.DataAnnotations;

namespace Lab_App.DTO
{
    public class LoginDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
