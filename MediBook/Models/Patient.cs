using System.ComponentModel.DataAnnotations.Schema;

namespace MediBook.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
