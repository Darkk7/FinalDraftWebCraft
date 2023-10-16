using Microsoft.AspNetCore.Mvc;
using VaccineProject.Data;
using VaccineProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Newtonsoft.Json;

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

        // Show a list of vaccines
        public IActionResult HomeVaccine()
        {
            IEnumerable<VaccineModel> objList = dbContext.vaccine;
            return View(objList);
        }

        // Show the form for adding a new vaccine
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of a new vaccine
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccineModel vaccine)
        {
            if (ModelState.IsValid)
            {
                // Check if the provided vaccine type exists
                var vaccineType = dbContext.vaccineType.FirstOrDefault(vt => vt.TypeName == vaccine.TypeName);
                if (vaccineType == null)
                {
                    ModelState.AddModelError("TypeName", "The provided Vaccine Type Name does not exist in the VaccineType model.");
                    return View(vaccine);
                }

                // Add the new vaccine to the database
                dbContext.vaccine.Add(vaccine);
                dbContext.SaveChanges();
                Console.WriteLine("Vaccine added successfully!"); 
                return RedirectToAction("HomeVaccine");
            }
            return View(vaccine);
        }

        // Show the form for updating a vaccine
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

        // Handle the update of a vaccine
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccineModel vaccine)
        {
            dbContext.vaccine.Update(vaccine);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccine");
        }

        // Handle the deletion of a vaccine
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

        // Show a dashboard with vaccine data and booking information
        public ActionResult Dashboard()
        {
            // Create a new DashboardViewModel instance
            var dashboardViewModel = new DashboardViewModel();

            // Fetch vaccine data from your database
            dashboardViewModel.Vaccines = dbContext.vaccine.ToList();

            // Get the booking count for today
            var today = DateTime.Now.Date;
            var todayBookings = dbContext.vaccineBookings.ToList();

            var bookingCount = todayBookings
                .Count(vb =>
                {
                    DateTime bookingDateDb;
                    return DateTime.TryParseExact(vb.BookingDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingDateDb) &&
                            bookingDateDb >= today && bookingDateDb < today.AddDays(1);
                });

            dashboardViewModel.Bookings = todayBookings;
            dashboardViewModel.BookingCount = bookingCount;

            // Retrieve the TotalRegisteredPatients count from the database
            dashboardViewModel.TotalRegisteredPatients = dbContext.VaccinePatients.Count();

            // Convert vaccine data to JSON and pass it to the view
            var vaccineData = dashboardViewModel.Vaccines.Select(vaccine => new
            {
                Name = vaccine.VaccineName,
                Quantity = vaccine.Quantity
            }).ToList();

            ViewBag.VaccineData = JsonConvert.SerializeObject(vaccineData);

            return View(dashboardViewModel);
        }

        // Update vaccine quantity when a booking is made
        [HttpPost]
        public IActionResult UpdateVaccineQuantity(int vaccineId, int doseNumber)
        {
            var vaccine = dbContext.vaccine.Find(vaccineId);

            if (vaccine != null)
            {
                if (vaccine.Quantity >= doseNumber)
                {
                    vaccine.Quantity -= doseNumber;
                    dbContext.vaccine.Update(vaccine);
                    dbContext.SaveChanges();

                    if (vaccine.Quantity <= vaccine.LowQuantity)
                    {
                        string alertMessage = $"Low quantity alert for {vaccine.VaccineName}. Current quantity: {vaccine.Quantity}";
                        TempData["LowQuantityAlert"] = alertMessage;
                    }

                    return RedirectToAction("Dashboard");
                }
            }

            return RedirectToAction("Dashboard");
        }

        // Show the login page
        public IActionResult Login()
        {
            return View();
        }

        // Authenticate the user based on a PIN
        [HttpPost]
        public IActionResult AuthenticatePin(DashboardPin model)
        {
            if (ModelState.IsValid)
            {
                string enteredPin = model.Pin.Trim();

                // Look for a user with the entered PIN in the database
                var user = dbContext.Pins.FirstOrDefault(e => e.Pin.Trim() == enteredPin);

                if (user != null)
                {
                    // User with matching PIN found, authenticate the user
                    // You can store user information in a session or cookie for subsequent requests
                    return RedirectToAction("Dashboard");
                }
            }

            ModelState.AddModelError("Pin", "Invalid PIN.");
            TempData["ErrorMessage"] = "Invalid PIN. Please check your PIN and try again.";
            return RedirectToAction("Login");
        }

        public IActionResult HomeOrders()
        {
            IEnumerable<OrderModel> objList = dbContext.Orders;
            return View(objList);
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(OrderModel order)
        {
            if (ModelState.IsValid)
            {
                // Add the new order to the database
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                // Retrieve the corresponding vaccine by name or ID
                var vaccine = dbContext.vaccine.FirstOrDefault(v => v.VaccineName == order.VaccineName);

                if (vaccine != null)
                {
                    // Update the vaccine's quantity
                    vaccine.Quantity += int.Parse(order.Quantity);
                    dbContext.vaccine.Update(vaccine);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("HomeOrders");
            }
            return View(order);
        }


        // Handle the deletion of a vaccine
        public IActionResult DeleteOrder(int? ID)
        {
            var obj = dbContext.Orders.Find(ID);

            if (obj == null)
            {
                return NotFound();
            }

            dbContext.Orders.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomeOrders");
        }
    }
}
