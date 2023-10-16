using Microsoft.AspNetCore.Mvc;
using VaccineProject.Data;
using VaccineProject.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using PuppeteerSharp;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VaccineWebApp.Controllers
{
    public class VaccineBookingController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<VaccineBookingController> _logger;

        // Constructor
        public VaccineBookingController(ILogger<VaccineBookingController> logger, ICompositeViewEngine viewEngine, ApplicationDbContext dBD)
        {
            _logger = logger;
            _viewEngine = viewEngine;
            dbContext = dBD;
        }

        // Display the booking homepage
        public IActionResult HomeBooking()
        {
            IEnumerable<VaccinationBookingModel> objList = dbContext.vaccineBookings
                .Include(vb => vb.Vaccine);
            return View(objList);
        }

        // Display the form for creating a new booking
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of a new booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccinationBookingModel vaccineBooking)
        {
            if (ModelState.IsValid)
            {
                // Check if the patient is registered
                var patient = dbContext.VaccinePatients.FirstOrDefault(p => p.PatientFullName == vaccineBooking.PatientFullName);

                if (patient != null)
                {
                    // Check if the vaccine exists
                    var vaccine = dbContext.vaccine.FirstOrDefault(v => v.VaccineName == vaccineBooking.VaccineName);

                    if (vaccine != null)
                    {
                        if (vaccine.Quantity >= vaccineBooking.DoseNumber)
                        {
                            // Calculate booking price and update vaccine stock
                            float bookingPrice = vaccine.Price * vaccineBooking.DoseNumber;
                            vaccine.Quantity -= vaccineBooking.DoseNumber;

                            dbContext.vaccine.Update(vaccine);
                            vaccineBooking.Price = bookingPrice;
                            dbContext.SaveChanges();

                            // Add the booking to the database
                            dbContext.vaccineBookings.Add(vaccineBooking);
                            dbContext.SaveChanges();

                            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                            {
                                // Return JSON for AJAX request
                                var updatedBookings = dbContext.vaccineBookings.ToList();
                                return Json(new { success = true, bookings = updatedBookings });
                            }

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
                else
                {
                    ModelState.AddModelError(string.Empty, "Patient is not registered. Please register the patient first.");
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Return JSON with validation errors for AJAX request
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }

            return View(vaccineBooking);
        }

        // Display the form for updating a booking
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

        // Handle the update of a booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccinationBookingModel vaccineBooking)
        {
            dbContext.vaccineBookings.Update(vaccineBooking);
            dbContext.SaveChanges();
            return RedirectToAction("HomeBooking");
        }

        // Handle the deletion of a booking
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

        // Count the bookings for a specific day
        public IActionResult CountBookingsForDay(string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate))
            {
                DateTime startOfDay = bookingDate.Date;
                DateTime endOfDay = startOfDay.AddDays(1);

                var bookingsForDate = dbContext.vaccineBookings.ToList();

                int bookingCount = bookingsForDate
                    .Count(vb => DateTime.TryParseExact(vb.BookingDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDateDb) &&
                                 bookingDateDb >= startOfDay && bookingDateDb < endOfDay);

                return Json(new { Date = date, BookingCount = bookingCount });
            }

            return Json(new { Error = "Invalid date format" });
        }

        // Display the vaccination details for a patient
        public IActionResult PatientVaccine()
        {
            IEnumerable<VaccinationBookingModel> objList = dbContext.vaccineBookings;
            return View(objList);
        }

        // Generate a patient report
        public IActionResult PatientReport(string patientFullName)
        {
            var patient = dbContext.VaccinePatients.FirstOrDefault(p => p.PatientFullName == patientFullName);

            if (patient != null)
            {
                var patientReportViewModel = new PatientReportViewModel
                {
                    PatientFullName = patient.PatientFullName,
                    DateOfBirth = patient.DateOfBirth,
                    DateRegistered = patient.DateAddedToSystem,
                    Allergies = patient.Allergies,
                    Gender = patient.Gender,
                    Email = patient.Email,
                };

                patientReportViewModel.VaccinationBookings = dbContext.vaccineBookings
                    .Where(booking => booking.PatientFullName == patientFullName)
                    .ToList();

                return View(patientReportViewModel);
            }

            return NotFound();
        }

        // Generate a patient report screenshot
        [HttpGet]
        public async Task<IActionResult> GeneratePatientReportScreenshot(string patientFullName)
        {
            // Log information about the action
            _logger.LogInformation("GeneratePatientReportScreenshot action called with patientFullName: {patientFullName}", patientFullName);

            // Path to the Chromium executable
            var chromiumExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            _logger.LogInformation("Chromium executable path: {chromiumExecutablePath}", chromiumExecutablePath);

            // Retrieve patient data based on the patientFullName parameter
            var patient = dbContext.VaccinePatients.FirstOrDefault(p => p.PatientFullName == patientFullName);

            if (patient != null)
            {
                // Create a ViewModel with patient data and related bookings
                var patientReportViewModel = new PatientReportViewModel
                {
                    PatientFullName = patient.PatientFullName,
                    DateOfBirth = patient.DateOfBirth,
                    DateRegistered = patient.DateAddedToSystem,
                    Allergies = patient.Allergies,
                    Gender = patient.Gender,
                    Email = patient.Email,
                    VaccinationBookings = dbContext.vaccineBookings
                        .Where(booking => booking.PatientFullName == patientFullName)
                        .ToList()
                };

                // Log patient data
                _logger.LogInformation("Patient data found: {patientFullName}", patientFullName);

                // Render the view to HTML
                var patientReportHtml = await this.RenderViewToStringAsync("PatientReport", patientReportViewModel);

                // Log the HTML content for debugging
                _logger.LogInformation("Rendered HTML: {html}", patientReportHtml);

                // Launch a headless Chromium browser
                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = chromiumExecutablePath
                });

                using var page = await browser.NewPageAsync();

                try
                {
                    // Set the page content to the generated HTML
                    await page.SetContentAsync(patientReportHtml);

                    // Wait for a specific element to ensure that the content is fully loaded
                    await page.WaitForSelectorAsync("vaccinationBookingsTable");

                    // Capture a screenshot
                    var screenshot = await page.ScreenshotBase64Async();

                    return File(Convert.FromBase64String(screenshot), "image/png");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error capturing screenshot: {exceptionMessage}", ex.Message);
                    return StatusCode(500);
                }
            }
            else
            {
                _logger.LogError("Patient data not found for patientFullName: {patientFullName}", patientFullName);
                return NotFound();
            }
        }

        // Render a view to a string
        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            using var writer = new StringWriter();
            var viewEngineResult = _viewEngine.FindView(ControllerContext, viewName, false);

            if (viewEngineResult.Success == false)
            {
                _logger.LogError("Could not find the view '{viewName}'", viewName);
                throw new InvalidOperationException($"Could not find the view '{viewName}'");
            }

            var view = viewEngineResult.View;

            // Create a new ViewDataDictionary and pass the model to it
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                ControllerContext,
                view,
                viewData, // Use the viewData with the model
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            await view.RenderAsync(viewContext);

            return writer.GetStringBuilder().ToString();
        }
    }
}
