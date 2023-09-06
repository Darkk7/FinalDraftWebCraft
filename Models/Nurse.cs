using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VaccineWebApp.Models
{
    public class Nurse
    {
        [Key]
        public int NurseID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
    }
}
