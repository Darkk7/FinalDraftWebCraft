using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineWebApp.Models
{
    public class VaccineTypeModel
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
