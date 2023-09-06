using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VaccinationSubsytem.Controllers
{
    public class VaccineController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public string ConnectionString = "Server=LAPTOP-TEOTC2HU;Database=GRP-03-07-WebCraft;Trusted_Connection=True";


        public VaccineController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }


        public IActionResult HomeVaccine()
        {
            IEnumerable<VaccineModel> objList = dbContext.vaccine;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccineModel vaccine)
        {
            if (ModelState.IsValid)
            {
                dbContext.vaccine.Add(vaccine);
                dbContext.SaveChanges();
                return RedirectToAction("HomeVaccine");
            }
            return View(vaccine);
        }

        public IActionResult Update(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return NotFound();
            }
            var obj = dbContext.vaccine.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccineModel vaccine)
        {
            dbContext.vaccine.Update(vaccine);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccine");
        }

        public IActionResult searchVaccine()
        {
            return View();
        }

        public IActionResult searchVaccineResult(VaccineModel vaccine)
        {
            int VacID = vaccine.VaccineID;
            string VacName = vaccine.Name;


            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand("sp_Vac_GetVaccineID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", VacName);

                    conn.Open();
                    VacID = (int)cmd.ExecuteScalar();
                    conn.Close();

                    TempData["Name"] = vaccine.Name;
                    TempData["DosesNeeded"] = vaccine.DosesNeeded;
                    TempData["Quantity"] = vaccine.Quantity;
                    TempData["LowQuantity"] = vaccine.LowQuantity;
                    TempData["TypeName"] = vaccine.TypeName;
                    TempData["MedAidNo"] = vaccine.MedAidNo;
                    TempData["Price"] = vaccine.Price;

                    return RedirectToAction("VaccineDetails");
                }
            }

       
        }

        public IActionResult VaccineDetails()
        {
            int vaccineID = Convert.ToInt32(TempData["VaccineID"]);
            ViewBag.VaccineName = TempData["Name"];
            ViewBag.DosesNeeded = TempData["DosesNeeded"];
            ViewBag.Quantity = TempData["Quantity"];
            ViewBag.LowQuantity = TempData["LowQuantity"];
            ViewBag.TypeName = TempData["TypeName"];
            ViewBag.MedAidNo = TempData["PatientMedAidNo"];
            ViewBag.Price = TempData["Price"];
            
            TempData.Clear();


            return View();

        }

        public IActionResult Delete(int? ID)
        {
            var obj = dbContext.vaccine.Find(ID);

            if (obj == null)
            {
                return NotFound();
            }

            dbContext.vaccine.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccine");
        }
    }
}
