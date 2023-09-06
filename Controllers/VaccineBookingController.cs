using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;


namespace VaccineWebApp.Controllers
{
    public class VaccineBookingController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public string ConnectionString = "Server=LAPTOP-TEOTC2HU;Database=GRP-03-07-WebCraft;Trusted_Connection=True";

        public VaccineBookingController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }


        public IActionResult HomeBooking()
        {
            IEnumerable<VaccinationBookingModel> objList = dbContext.vaccineBookings;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccinationBookingModel vaccineBooking)
        {
            if (ModelState.IsValid)
            {
                dbContext.vaccineBookings.Add(vaccineBooking);
                dbContext.SaveChanges();
                return RedirectToAction("HomeBooking");
            }
            return View(vaccineBooking);
        }

        public IActionResult searchVaccineBooking()
        {
            return View();
        }

        public IActionResult searchVaccineBookingResult(VaccinationBookingModel vaccineBooking)
        {
            int ID = vaccineBooking.PatientID;

            int VaccineBookingID;

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand("sp_Vac_GetVacBookID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientID", ID);

                    conn.Open();
                    VaccineBookingID = (int)cmd.ExecuteScalar();
                    conn.Close();

                    return View("searchVaccineBooking");
                }
            }
        }

       

        public IActionResult Update(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return NotFound();
            }
            var obj = dbContext.vaccineBookings.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccinationBookingModel vaccineBooking)
        {
            dbContext.vaccineBookings.Update(vaccineBooking);
            dbContext.SaveChanges();
            return RedirectToAction("HomeBooking");
        }

        public IActionResult Delete(int? ID)
        {
            var obj = dbContext.vaccineBookings.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            dbContext.vaccineBookings.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomeBooking");
        }

        public IActionResult searchPatient()
        {
            return View();
        }
    }
}
