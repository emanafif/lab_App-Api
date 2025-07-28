using System.ComponentModel.DataAnnotations;

namespace Lab_App.DTO
{
    public class allexpermentsDTO
    {
        public int Id { get; set; }

        public double? BOD { get; set; }
        public double? Qin { get; set; }
        public double? MLSS { get; set; }
        public double? MLSSwas { get; set; }

        public double? Na { get; set; }
        public double? Va { get; set; }
        public double? FM { get; set; }
        public double? Qpumpreturn { get; set; }
        public double? Qpumpplus { get; set; }
        public double? MLSSB { get; set; }
        public double? Mx { get; set; }
        public double? MB { get; set; }
        public double? M { get; set; }
        public double? Qrasplus { get; set; }
        public double? Qrasminus { get; set; }

        public double? QrasTotal { get; set; }
        public double? Qtimeras { get; set; }

        public double? QrasPERSCENT { get; set; }
        public double? WantedQrasPERSCENT { get; set; }
        public DateTime? date { get; set; }
        public string labname { get; set; }
        public int labId { get; set; }

    }
}
