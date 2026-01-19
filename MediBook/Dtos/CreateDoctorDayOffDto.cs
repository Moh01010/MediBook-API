namespace MediBook.Dtos
{
    public class CreateDoctorDayOffDto
    {
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
    }
}
