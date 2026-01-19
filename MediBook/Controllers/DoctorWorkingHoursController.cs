using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediBook.Interface;
using MediBook.Dtos;
using System.Threading.Tasks;
using MediBook.Models;
namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorWorkingHoursController : ControllerBase
    {
        private readonly IDoctorWorkingHourRepository _workRepo;
        private readonly IDoctorRepository _doctorRepo;

        public DoctorWorkingHoursController(IDoctorWorkingHourRepository workRepo, IDoctorRepository doctorRepo)
        {
            _workRepo = workRepo;
            _doctorRepo = doctorRepo;
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(int doctorId, CreateDoctorWorkingHourDto dto)
        {
            var doctor=await _doctorRepo.GetByIdAsync(doctorId);
            if (doctor == null)
                return NotFound("Doctor not found");
            if(dto.StartTime>dto.EndTime)
                return BadRequest("Invalid time range");
            var workingHour = new DoctorWorkingHour
            {
                DoctorId = doctorId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };
            await _workRepo.AddAsync(workingHour);
            return Ok("Working hour added successfully");
        }
        [HttpGet]
        public async Task<IActionResult> Get(int doctorId)
        {
            var hours=await _workRepo.GetByDoctorIdAsync(doctorId);

            var result = hours.Select(h => new DoctorWorkingHourDto
            {
                DayOfWeek = h.DayOfWeek,
                StartTime = h.StartTime,
                EndTime = h.EndTime
            });
            return Ok(result);
        }
    }
}
