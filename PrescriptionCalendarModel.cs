using System.ComponentModel.DataAnnotations;

namespace Chronic_Medication_system.Models
{
    public class PrescriptionCalendarModel
    {
        [Key]
        public int PatientID { get; set; }
        [Required]
        public string? PrescriptionName { get; set; }

        [Required]
        public DateTime prescription_end_date { get; set; }

        [Required]
        public string? eventDesc { get; set; }  
    }
}
