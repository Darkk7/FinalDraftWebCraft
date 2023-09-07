using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineWebApp.Models
{
    public class VaccinationBookingModel
    {
        [Key]
        public int VaccineBookingID { get; set; }
        public int PatientID { get; set; }
        public int VaccineID { get; set; }
        public string? BookingDate { get; set; } // Add '?' to make it nullable
        public int DoseNumber { get; set; }

        [ForeignKey("VaccineID")]
        public VaccineModel? Vaccine { get; set; }
    }

}
