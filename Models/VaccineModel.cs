using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineWebApp.Models
{
    public class VaccineModel
    {
        [Key]
        public int VaccineID { get; set; }
        public string Name { get; set; } 
        public int DosesNeeded { get; set; }
        public int Quantity { get; set; } 
        public int LowQuantity { get; set; }
        public string TypeName { get; set; }
        public string MedAidNo { get; set; } 
        public float Price { get; set; } 
    }
}

    
