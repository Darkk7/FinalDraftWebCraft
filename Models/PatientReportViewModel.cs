namespace VaccineProject.Models
{
    public class PatientReportViewModel
    {
            public string PatientFullName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime DateRegistered { get; set; }
            public string Allergies { get; set; }
            public string Gender { get; set; }
            public string Email { get; set; }

        public List<VaccinationBookingModel> VaccinationBookings { get; set; }
    }
}
