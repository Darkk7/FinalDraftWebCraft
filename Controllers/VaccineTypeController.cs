using Microsoft.AspNetCore.Mvc;
using VaccineProject.Data;
using VaccineProject.Models;

namespace VaccinationSubsytem.Controllers
{
    public class VaccineTypeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public VaccineTypeController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }

        // Show a list of vaccine types
        public IActionResult HomeVaccineType()
        {
            IEnumerable<VaccineTypeModel> objList = dbContext.vaccineType;
            return View(objList);
        }

        // Show the form for adding a new vaccine type
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of a new vaccine type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccineTypeModel vaccineType)
        {
            if (ModelState.IsValid)
            {
                // Add the new vaccine type to the database
                dbContext.vaccineType.Add(vaccineType);
                dbContext.SaveChanges();
                return RedirectToAction("HomeVaccineType");
            }
            return View(vaccineType);
        }

        // Show the form for updating a vaccine type
        public IActionResult Update(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return NotFound();
            }
            var obj = dbContext.vaccineType.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // Handle the update of a vaccine type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccineTypeModel vaccineType)
        {
            dbContext.vaccineType.Update(vaccineType);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccineType");
        }

        // Handle the deletion of a vaccine type
        public IActionResult Delete(int? ID)
        {
            var obj = dbContext.vaccineType.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            dbContext.vaccineType.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccineType");
        }
    }
}
