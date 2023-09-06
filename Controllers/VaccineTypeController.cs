using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;

namespace VaccinationSubsytem.Controllers
{
    public class VaccineTypeController : Controller
    {

        private readonly ApplicationDbContext dbContext;

        public VaccineTypeController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }


        public IActionResult HomeVaccineType()
        {
            IEnumerable<VaccineTypeModel> objList = dbContext.vaccineType;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }


        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VaccineTypeModel vaccineType)
        {
            if (ModelState.IsValid)
            {
                dbContext.vaccineType.Add(vaccineType);
                dbContext.SaveChanges();
                return RedirectToAction("HomeVaccineType");
            }
            return View(vaccineType);
        }


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VaccineTypeModel vaccineType)
        {
            dbContext.vaccineType.Update(vaccineType);
            dbContext.SaveChanges();
            return RedirectToAction("HomeVaccineType");
        }

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
