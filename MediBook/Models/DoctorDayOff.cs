using System.ComponentModel.DataAnnotations.Schema;

namespace MediBook.Models
{
    public class DoctorDayOff
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public string? Reason { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId {  get; set; }
        public Doctor Doctor { get; set; }

    }
}
