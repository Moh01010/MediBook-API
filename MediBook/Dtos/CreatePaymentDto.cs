namespace MediBook.Dtos
{
    public class CreatePaymentDto
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
    }
}

