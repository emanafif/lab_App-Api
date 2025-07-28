using Lab_App.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lab_App.DTO
{
    public class LabDTO
    {
        
        public string lab_name { get; set; }
        public int Na { get; set; }
        public int Va { get; set; }
        public double Qras { get; set; }
        public double? fm { get; set; }
        public double? Qpumpreturn { get; set; }
        public double? Qpumpplus { get; set; }
        [JsonIgnore]
        public ICollection<mainTable>? MainTables { get; set; }

    }




    public class getLabbyidDTO
    {
        public int lab_id { get; set; }

        public string lab_name { get; set; }
        public int Na { get; set; }
        public int Va { get; set; }
        public double Qras { get; set; }
        public double? fm { get; set; }
        public double? Qpumpreturn { get; set; }
        public double? Qpumpplus { get; set; }
        [JsonIgnore]
        public ICollection<mainTable>? MainTables { get; set; }

    }
}
