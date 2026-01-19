using System.ComponentModel.DataAnnotations.Schema;

namespace MediBook.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Specialty { get; set; }
        public string Bio { get; set; }
        public int SlotDuration { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        [ForeignKey("ApplicationUser")]
        public string UserId {  get; set; }
        public  ApplicationUser User { get; set; }
        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public  Clinic Clinic { get; set; }
        public ICollection<DoctorWorkingHour> WorkingHours { get; set; }
        public ICollection<DoctorDayOff> DaysOff { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
