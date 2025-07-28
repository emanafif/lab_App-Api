using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Lab_App.Models
{
    public class Labs
    {
        [Key]
        public int lab_id { get; set; }
        public string lab_name { get; set; }
        public int Na { get; set; }
        public int Va { get; set; }
        public double Qras { get; set; }
        public double? fm { get; set; }
        public double? Qpumpreturn { get; set; }
        public double? Qpumpplus { get; set; }

        public ICollection<mainTable> MainTables { get; set; }

    }
}
