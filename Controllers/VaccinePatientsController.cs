using Microsoft.AspNetCore.Mvc;
using VaccineProject.Data;
using VaccineProject.Models;
using System.Linq;

namespace VaccineWebApp.Controllers
{
    public class VaccinePatientsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private int TotalRegisteredPatients { get; set; }

        public VaccinePatientsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            // Get the total count of registered patients from the database
            TotalRegisteredPatients = dbContext.VaccinePatients.Count();
        }

        // Show a list of registered patients
        public IActionResult HomePatients()
        {
            var objList = dbContext.VaccinePatients.ToList();
            return View(objList);
        }

        // Show the form for adding a new patient
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of a new patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccinePatients patients)
        {
            if (ModelState.IsValid)
            {
                // Add the new patient to the database
                dbContext.VaccinePatients.Add(patients);
                dbContext.SaveChanges();

                // Retrieve the current TotalRegisteredPatients count from the database
                int totalRegisteredPatients = dbContext.VaccinePatients.Count();

                // Redirect to the view with the updated TotalRegisteredPatients count
                TempData["TotalRegisteredPatients"] = totalRegisteredPatients;

                return RedirectToAction("HomePatients");
            }
            return View(patients);
        }


        // Show the form for updating a patient's details
        public IActionResult Update(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return NotFound();
            }
            var obj = dbContext.VaccinePatients.Find(ID);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // Handle the update of a patient's details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccinePatients patients)
        {
            dbContext.VaccinePatients.Update(patients);
            dbContext.SaveChanges();
            return RedirectToAction("HomePatients");
        }

        // Handle the deletion of a patient
        public IActionResult Delete(int? ID)
        {
            var obj = dbContext.VaccinePatients.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            dbContext.VaccinePatients.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomePatients");
        }

        // Retrieve a list of registered patients and return it as JSON
        public IActionResult GetRegisteredPatients()
        {
            var registeredPatients = dbContext.VaccinePatients.ToList();
            return Json(registeredPatients);
        }
    }
}
