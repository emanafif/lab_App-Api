using Lab_App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lab_App.DTO
{
    public class mainDTO
    {

        public double? BOD { get; set; }
        public double? Qin { get; set; }
        public double? MLSS { get; set; }
        public double? MLSSwas { get; set; }

        public DateTime? date { get; set; }
        public int Na { get; set; }
        public int Va { get; set; }
        public double Qras { get; set; }
        public double? Tss { get; set; }

        public double? fm { get; set; }
        public int labId { get; set; }
        [ForeignKey("labId")]

        [JsonIgnore]
        public virtual Labs? Labs { get; set; }

    }
}
