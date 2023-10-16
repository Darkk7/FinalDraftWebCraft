using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineProject.Models
{
    public class VaccinePatients
    {
        [Key]
        public int VaccinePatientID { get; set; }

        [Required(ErrorMessage = "Patient Full Name is required.")]
        public string PatientFullName { get; set; }

        [Required(ErrorMessage = "ID Number is required.")]
        [StringLength(13, ErrorMessage = "ID Number must not exceed 13 characters.")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAddedToSystem { get; set; }

        public string Allergies { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }
    }

}
