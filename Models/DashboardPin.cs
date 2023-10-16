using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineProject.Models
{
    public class DashboardPin
    {
        [Key]
        public int PinID { get; set; }

        public string Pin { get; set; }
    }
}
