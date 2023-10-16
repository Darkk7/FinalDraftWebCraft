using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineProject.Models
{
    public class VaccineModel
    {
        [Key]
        public int VaccineID { get; set; }

        [Required(ErrorMessage = "Vaccine Name is required.")]
        public string VaccineName { get; set; }

        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Doses Needed is required.")]
        public int DosesNeeded { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Low Quantity is required.")]
        public int LowQuantity { get; set; }

        [Required(ErrorMessage = "Type Name is required.")]
        public string TypeName { get; set; }

        [Required(ErrorMessage = "Medical Aid Number is required.")]
        public string MedAidNo { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public float Price { get; set; }

        [ForeignKey("TypeID")]
        public VaccineTypeModel? VaccineType { get; set; }
    }
}

    
