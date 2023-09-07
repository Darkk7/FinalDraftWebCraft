using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace VaccinationSubsytem.Controllers
{
    public class VaccineController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public string ConnectionString = "Server=SICT-SQL.mandela.ac.za;Database=GRP-03-07-WebCraft;User ID=GRP-03-07;pwd=grp-03-07-soit2023;Trusted_Connection=False;";


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
            int? vaccineID = TempData["VaccineID"] as int?;
            if (vaccineID != null)
            {
                ViewBag.VaccineName = TempData["Name"];
                ViewBag.DosesNeeded = TempData["DosesNeeded"];
                ViewBag.Quantity = TempData["Quantity"];
                ViewBag.LowQuantity = TempData["LowQuantity"];
                ViewBag.TypeName = TempData["TypeName"];
                ViewBag.MedAidNo = TempData["PatientMedAidNo"];
                ViewBag.Price = TempData["Price"];
            }

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


        public ActionResult Dashboard()
        {
            List<VaccineModel> vaccines = dbContext.vaccine.ToList();

            int totalQuantity = vaccines.Sum(v => v.Quantity);

            // Check if there's a low quantity alert message in TempData
            string lowQuantityAlert = TempData["LowQuantityAlert"] as string;

            if (!string.IsNullOrEmpty(lowQuantityAlert))
            {
                ViewBag.LowQuantityAlert = lowQuantityAlert;

                // Clear the alert from TempData to prevent it from displaying multiple times
                TempData.Remove("LowQuantityAlert");
            }

            // Get the booking count for today (you can replace "DateTime.Now.Date" with your desired date)
            var today = DateTime.Now.Date;

            // Retrieve the bookings for today from the database
            var todayBookings = dbContext.vaccineBookings.ToList();

            // Filter and count the bookings for the specified date
            var bookingCount = todayBookings
                .Count(vb =>
                {
                    DateTime bookingDateDb;
                    return DateTime.TryParseExact(vb.BookingDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingDateDb) &&
                           bookingDateDb >= today && bookingDateDb < today.AddDays(1);
                });

            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.BookingCount = bookingCount;

            return View(vaccines);
        }



        // Add this method to update vaccine quantity when a booking is made
        [HttpPost]
        public IActionResult UpdateVaccineQuantity(int vaccineId, int doseNumber)
        {
            var vaccine = dbContext.vaccine.Find(vaccineId);

            if (vaccine != null)
            {
                // Check if there are enough vaccines in stock
                if (vaccine.Quantity >= doseNumber)
                {
                    // Decrement the vaccine quantity
                    vaccine.Quantity -= doseNumber;

                    // Update the vaccine quantity in the database
                    dbContext.vaccine.Update(vaccine);

                    // Save changes to the database
                    dbContext.SaveChanges();

                    // Check if the quantity is now below the low quantity threshold
                    if (vaccine.Quantity <= vaccine.LowQuantity)
                    {
                        // Create an alert message or log the alert as needed
                        string alertMessage = $"Low quantity alert for {vaccine.Name}. Current quantity: {vaccine.Quantity}";

                        // You can store this alert message in a database or a log file for future reference

                        // For now, let's use TempData to pass the alert message to the Dashboard
                        TempData["LowQuantityAlert"] = alertMessage;
                    }

                    return RedirectToAction("Dashboard");
                }
            }

            return RedirectToAction("Dashboard");
        }


    }
}
