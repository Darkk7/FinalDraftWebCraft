using System.ComponentModel.DataAnnotations;

namespace Chronic_Medication_system.Models
{
    public class PrescriptionBookingModel
    {
        [Key]
        public int BookingID { get; set; }
        [Required]
        public int PatientID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime prescription_end_date { get; set; }
        [Required]
        public string? reason { get; set; }
        [Required]

        public string? MedicationName { get; set; }
        [Required]
        public int totalAmount { get; set; }



    }
}
