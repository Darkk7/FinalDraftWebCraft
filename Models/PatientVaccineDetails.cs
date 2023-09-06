using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineWebApp.Models
{
    public class PatientVaccineDetails
    {
        [Key]
        public int PatientVaccineID { get; set; }
        public int PatientID { get; set; }
        public int VaccineID { get; set; }
        public int NumberOfDoses { get; set; }
        
    }
}
