namespace MediBook.Dtos
{
    public class DoctorDayOffDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
    }
}
