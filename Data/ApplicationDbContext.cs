using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VaccineWebApp.Models;

namespace VaccineWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Nurse> Nurse { get; set; }
        public DbSet<VaccineModel> vaccine { get; set; }
        public DbSet<VaccinationBookingModel> vaccineBookings { get; set; }
        public DbSet<VaccineTypeModel> vaccineType { get; set; }
        public DbSet<PatientVaccineDetails> patientVaccineDetails { get; set; }
    }
}