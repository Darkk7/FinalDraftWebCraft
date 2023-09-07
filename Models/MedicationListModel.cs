using System.ComponentModel.DataAnnotations;

namespace Chronic_Medication_system.Models

{
    public class MedicationListModel
    {

        [Key]
        
        public int MedicationID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Low_Quantity { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]

        public int Price{ get; set; }
        [Required]
        public string? BrandName { get; set; }
        [Required]
        public string? MedicalAid { get; set; }
    }
}
