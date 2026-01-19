using MediBook.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediBook.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date {  get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public Payment Payment {  get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor {  get; set; }
        [ForeignKey("Patient")]
        public int PatientId {  get; set; }
        public Patient Patient { get; set; }


    }
}
