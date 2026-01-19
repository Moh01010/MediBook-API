namespace MediBook.Dtos
{
    public class CreateDoctorDto
    {
        public string Email { get; set; }
        public int ClinicId { get; set; }
        public string Specialty { get; set; }
        public string Bio { get; set; }
        public int SlotDuration { get; set; }
    }
}
