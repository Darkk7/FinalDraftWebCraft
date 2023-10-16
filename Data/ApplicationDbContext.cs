using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VaccineProject.Models;

namespace VaccineProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VaccineModel> vaccine { get; set; }
        public DbSet<VaccinationBookingModel> vaccineBookings { get; set; }
        public DbSet<VaccineTypeModel> vaccineType { get; set; }
        public DbSet<VaccinePatients> VaccinePatients { get; set; }
        public DbSet<DashboardPin> Pins { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
    }
}