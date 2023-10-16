namespace VaccineProject.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<VaccinationBookingModel> Bookings { get; set; }
        public IEnumerable<VaccineModel> Vaccines { get; set; }

        public int TotalQuantity { get; set; }
        public int BookingCount { get; set; }
        public int TotalRegisteredPatients { get; set; }
    }
}
