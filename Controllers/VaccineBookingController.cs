using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Globalization; 
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
        public string ConnectionString = "Server=SICT-SQL.mandela.ac.za;Database=GRP-03-07-WebCraft;User ID=GRP-03-07;pwd=grp-03-07-soit2023;Trusted_Connection=False;";

        public VaccineBookingController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }

        public IActionResult HomeBooking()
        {
            IEnumerable<VaccinationBookingModel> objList = dbContext.vaccineBookings

                .Include(vb => vb.Vaccine);
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
                // Find the vaccine associated with the booking
                var vaccine = dbContext.vaccine.Find(vaccineBooking.VaccineID);

                if (vaccine != null)
                {
                    // Check if there are enough vaccines in stock
                    if (vaccine.Quantity >= vaccineBooking.DoseNumber)
                    {
                        // Decrement the total quantity of vaccines
                        vaccine.Quantity -= vaccineBooking.DoseNumber;

                        // Update the vaccine quantity in the database
                        dbContext.vaccine.Update(vaccine);

                        // Save changes to the database
                        dbContext.SaveChanges();

                        // Create the booking
                        dbContext.vaccineBookings.Add(vaccineBooking);
                        dbContext.SaveChanges();

                        return RedirectToAction("HomeBooking");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Not enough vaccines in stock.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Vaccine not found.");
                }
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

        public IActionResult CountBookingsForDay(string date)
        {
            // Parse the date string to a DateTime object
            if (DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate))
            {
                // Calculate the start and end of the day for comparison
                DateTime startOfDay = bookingDate.Date;
                DateTime endOfDay = startOfDay.AddDays(1);

                // Retrieve data from the database
                var bookingsForDate = dbContext.vaccineBookings.ToList();

                // Filter and count the bookings for the specified date
                int bookingCount = bookingsForDate
                    .Count(vb => DateTime.TryParseExact(vb.BookingDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDateDb) &&
                                 bookingDateDb >= startOfDay && bookingDateDb < endOfDay);

                return Json(new { Date = date, BookingCount = bookingCount });
            }

            return Json(new { Error = "Invalid date format" });
        }




    }
}
