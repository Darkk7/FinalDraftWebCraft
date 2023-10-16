using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineProject.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderID { get; set; }

        public string VaccineName { get; set; }

        public string CompanyName { get; set; }

        public string Quantity { get; set; }

        [DataType(DataType.Date)]
        public string? OrderDate { get; set; }

        [ForeignKey("VaccineID")]
        public VaccineModel? Vaccine { get; set; }
    }
}
