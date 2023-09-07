using Microsoft.AspNetCore.Mvc;
using Chronic_Medication_system.Models;
using Chronic_Medication_system.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Chronic_Medication_system.Controllers
{
    public class MedicationListController : Controller
    {
        private static List<MedicationListModel> show = new List<MedicationListModel>();

        private readonly ApplicationDbContext dbContext;
        public string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-Chronic_Medication_system-4fdc75a4-444f-410e-ad3a-88315b16b7cd;Trusted_Connection=False;MultipleActiveResultSets=true;";
        public MedicationListController
           (ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }
        public IActionResult MedicationListView()
        {
           
            return View();
        }
    }
}
