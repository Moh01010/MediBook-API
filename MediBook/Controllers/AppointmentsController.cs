using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediBook.Interface;
using MediBook.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;
using MediBook.Models;
using MediBook.Enum;
using Microsoft.EntityFrameworkCore;
using MediBook.Repository;
namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly IDoctorDayOffRepository _doctorDayOffRepo;
        private readonly IDoctorWorkingHourRepository _workingHourRepo;
        private readonly INotificationService _notificationService;

        public AppointmentsController(IAppointmentRepository appointmentRepo, IDoctorRepository doctorRepo, IPatientRepository patientRepo, IDoctorDayOffRepository doctorDayOffRepo, IDoctorWorkingHourRepository workingHourRepo, INotificationService notificationService)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
            _doctorDayOffRepo = doctorDayOffRepo;
            _workingHourRepo = workingHourRepo;
            _notificationService = notificationService;
        }

        [HttpPost("Booking")]
        public async Task<IActionResult> Booking(CreateAppointmentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _patientRepo.GetByUserIdAsync(userId);
            if (patient == null)
            {
                patient = new Patient { UserId = userId };
                await _patientRepo.CreateAsync(patient);
            }
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return NotFound("Doctor not found");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dto.Date < today)
                return BadRequest("Cannot book appointment in the past");

            if (dto.Date == today && dto.StartTime <= DateTime.UtcNow.TimeOfDay)
                return BadRequest("Cannot book appointment in the past");

            var isDayOff = await _doctorDayOffRepo.ExistsAsync(doctor.Id, dto.Date);
            if (isDayOff)
                return BadRequest("Doctor is off on this day");

            var dayOfWeek = dto.Date.DayOfWeek;
            var workingHours = await _workingHourRepo
                .GetByDoctorAndDayAsync(dto.DoctorId, dayOfWeek);

            if (!workingHours.Any())
                return BadRequest("Doctor does not work on this day");

            var endTime = dto.StartTime + TimeSpan.FromMinutes(doctor.SlotDuration);

            var validTime = workingHours.Any(wh =>dto.StartTime >= wh.StartTime &&endTime <= wh.EndTime);

            if (!validTime)
                return BadRequest("Time is outside working hours");

            var available = await _appointmentRepo.IsSlotAvailableAsync(doctor.Id, dto.StartTime, endTime, dto.Date);
            if (!available)
                return BadRequest("This time slot is already booked");

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                StartTime = dto.StartTime,
                EndTime = endTime,
                Date = dto.Date,
                Status = AppointmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await _appointmentRepo.CreateAsync(appointment);

            await _notificationService.NotifyAsync(
            doctor.UserId,
            "New Appointment",
            "You have a new appointment booking.");

            return Ok("Appointment booked successfully");
        }
        [HttpGet("my")]
        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _appointmentRepo.GetPatientAppointmentsAsync(userId);
            var result = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorName = a.Doctor.User.FullName,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Date = a.Date,
                Status = a.Status
            });
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepo.SaveAsync();

            return Ok("Appointment cancelled");

        }
    }
}
