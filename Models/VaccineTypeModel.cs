using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineProject.Models
{
    public class VaccineTypeModel
    {
        [Key]
        public int TypeID { get; set; }

        [Required(ErrorMessage = "Type Name is required.")]
        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}
