namespace MediBook.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
