using MediBook.Dtos;
using MediBook.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly IDoctorAvailableRepository _repo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IDoctorDayOffRepository _doctorDayOffRepo;
        private readonly IDoctorWorkingHourRepository _workingHourRepo;
        private readonly IAppointmentRepository _appointmentRepo;

        public DoctorAvailabilityController(IDoctorAvailableRepository repo, IDoctorRepository doctorRepo, IDoctorDayOffRepository doctorDayOffRepo, IDoctorWorkingHourRepository workingHourRepo, IAppointmentRepository appointmentRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _doctorDayOffRepo = doctorDayOffRepo;
            _workingHourRepo = workingHourRepo;
            _appointmentRepo = appointmentRepo;
        }

        [HttpGet("{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, [FromQuery] DateOnly date)
        {
            var doctor=await _doctorRepo.GetByIdAsync(doctorId);
            if (doctor == null) 
                return NotFound("Doctor not found");

            var isDayOff=await _doctorDayOffRepo.ExistsAsync(doctorId, date);
            if (isDayOff)
                return Ok(new List<AvailableSlotDto>());

            var dayOfWeek=date.DayOfWeek;
            var workingHours= await _workingHourRepo.GetByDoctorIdAsync(doctorId);

            var todayHours=workingHours.Where(w=>w.DayOfWeek==dayOfWeek).ToList();
            if (!todayHours.Any())
                return Ok(new List<AvailableSlotDto>());

            var appointments = await _repo.GetDoctorAppointmentsByDateAsync(doctorId, date);

            var slots=new List<AvailableSlotDto>();

            foreach (var wh in todayHours)
            {
                var current = wh.StartTime;
                var end = wh.EndTime;

                while (current + TimeSpan.FromMinutes(doctor.SlotDuration) <= end)
                {
                    var slotEnd = current + TimeSpan.FromMinutes(doctor.SlotDuration);

                    var overlaps = appointments.Any(a =>
                        a.StartTime < slotEnd &&
                        a.EndTime > current
                    );

                    if (!overlaps)
                    {
                        slots.Add(new AvailableSlotDto
                        {
                            StartTime = current,
                            EndTime = slotEnd
                        });
                    }

                    current = slotEnd;
                }
            }
            return Ok(slots);
        }
    }
}
