using MediBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MediBook.Data
{
    public class MediBookContext : IdentityDbContext<ApplicationUser>
    {
        public MediBookContext(DbContextOptions<MediBookContext> options)
         : base(options)
        {
        }

        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorWorkingHour> DoctorWorkingHours { get; set; }
        public DbSet<DoctorDayOff> DoctorDaysOff { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DateOnly conversion
            modelBuilder.Entity<Appointment>()
                .Property(a => a.Date)
                .HasConversion(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d)
                );

            ConfigureClinic(modelBuilder);
            ConfigureDoctor(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureAppointment(modelBuilder);
            ConfigurePayment(modelBuilder);
        }


        private void ConfigureClinic(ModelBuilder builder)
        {
            builder.Entity<Clinic>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.Phone)
                    .HasMaxLength(20);
            });
        }
        private void ConfigureDoctor(ModelBuilder builder)
        {
            builder.Entity<Doctor>(entity =>
            {
                entity.Property(d => d.Specialty)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.SlotDuration)
                    .IsRequired();

                // Doctor ↔ User (1–1)
                entity.HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<Doctor>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Doctor ↔ Clinic (Many–1)
                entity.HasOne(d => d.Clinic)
                    .WithMany(c => c.Doctors)
                    .HasForeignKey(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private void ConfigurePatient(ModelBuilder builder)
        {
            builder.Entity<Patient>(entity =>
            {
                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<Patient>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private void ConfigureAppointment(ModelBuilder builder)
        {
            builder.Entity<Appointment>(entity =>
            {
                entity.Property(a => a.Status)
                    .IsRequired();

                // Doctor ↔ Appointment
                entity.HasOne(a => a.Doctor)
                    .WithMany(d => d.Appointments)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Patient ↔ Appointment
                entity.HasOne(a => a.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                //  Prevent Double Booking (logic-level index)
                entity.HasIndex(a => new
                {
                    a.DoctorId,
                    a.Date,
                    a.StartTime
                }).IsUnique();
            });
        }
        private void ConfigurePayment(ModelBuilder builder)
        {
            builder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount)
                    .HasPrecision(18, 2);

                entity.HasOne(p => p.Appointment)
                    .WithOne(a => a.Payment)
                    .HasForeignKey<Payment>(p => p.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
