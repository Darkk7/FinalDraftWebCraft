using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineWebApp.Models
{
    public class VaccinationBookingModel
    {
        [Key]
        public int VaccineBookingID { get; set; }
        public int PatientID { get; set; }
        public int VaccineID { get; set; }
        public string BookingDate { get; set; }
        public int DoseNumber { get; set; }
    }
}
