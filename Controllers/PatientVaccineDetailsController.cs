using Microsoft.AspNetCore.Mvc;
using VaccineWebApp.Data;
using VaccineWebApp.Models;

namespace VaccinationSubsytem.Controllers
{
    public class PatientVaccineDetailsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PatientVaccineDetailsController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }


        public IActionResult HomePatientVaccine()
        {
            IEnumerable<PatientVaccineDetails> objList = dbContext.patientVaccineDetails;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientVaccineDetails patientVaccine)
        {
            if (ModelState.IsValid)
            {
                dbContext.patientVaccineDetails.Add(patientVaccine);
                dbContext.SaveChanges();
                return RedirectToAction("HomePatientVaccine");
            }
            return View(patientVaccine);
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
        public IActionResult Update(PatientVaccineDetails patientVaccine)
        {
            dbContext.patientVaccineDetails.Update(patientVaccine);
            dbContext.SaveChanges();
            return RedirectToAction("HomePatientVaccine");
        }

        public IActionResult Delete(int? ID)
        {
            var obj = dbContext.patientVaccineDetails.Find(ID);
            if (obj == null)
            {
                return NotFound();
            }

            dbContext.patientVaccineDetails.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("HomePatientVaccine");
        }
    }
}
