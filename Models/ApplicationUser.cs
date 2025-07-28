using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LicenseKey { get; set; } 
        public virtual License License { get; set; }
        [ForeignKey("Labs")]

        public int labId { get; set; }
        public  Labs Labs { get; set; }

    }
}
