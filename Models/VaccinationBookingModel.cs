using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineProject.Models
{
    public class VaccinationBookingModel
    {
        [Key]
        public int VaccineBookingID { get; set; }

        [Required(ErrorMessage = "Patient Full Name is required.")]
        public string PatientFullName { get; set; }

        public string NurseFullName { get; set; }

        [Required(ErrorMessage = "Vaccine Name is required.")]
        public string VaccineName { get; set; }

        [DataType(DataType.Date)]
        public string? BookingDate { get; set; }

        [Required(ErrorMessage = "Dose Number is required.")]
        public int DoseNumber { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public float Price { get; set; }

        [NotMapped]
        public int NumberOfDoses { get; set; }

        [ForeignKey("VaccineID")]
        public VaccineModel? Vaccine { get; set; }

        [ForeignKey("VaccinePatientID")]
        public VaccinePatients? VaccinePatients { get; set; }
    }


}
