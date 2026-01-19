using MediBook.Enum;
using System.ComponentModel.DataAnnotations.Schema;
namespace MediBook.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount {  get; set; }
        public PaymentStatus Status {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
