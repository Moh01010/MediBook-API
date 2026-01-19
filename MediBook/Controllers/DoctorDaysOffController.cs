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
    public class DoctorDaysOffController : ControllerBase
    {
        private readonly IDoctorDayOffRepository _doctorDayOffRepo;
        private readonly IDoctorRepository _doctorRepo;

        public DoctorDaysOffController(IDoctorDayOffRepository doctorDayOffRepo, IDoctorRepository doctorRepo)
        {
            _doctorDayOffRepo = doctorDayOffRepo;
            _doctorRepo = doctorRepo;
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(int doctorId,CreateDoctorDayOffDto dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync(doctorId);
            if (doctor == null)
                return NotFound("Doctor not found");
            var exists = await _doctorDayOffRepo.ExistsAsync(doctorId,dto.Date);
            if (exists)
                return BadRequest("Day off already exists");
            var dayOff = new DoctorDayOff
            {
                DoctorId = doctorId,
                Date = dto.Date,
                Reason = dto.Reason
            };
            await _doctorDayOffRepo.AddAsync(dayOff);
            return Ok("Day off added successfully");
        }
        [HttpGet]
        public async Task<IActionResult> Get(int doctorId)
        {
            var days = await _doctorDayOffRepo.GetByDoctorIdAsync(doctorId);
            var result = days.Select(d => new DoctorDayOffDto
            {
                Id = d.Id,
                Date = d.Date,
                Reason = d.Reason
            });
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int doctorId,int id)
        {
            var days = await _doctorDayOffRepo.GetByDoctorIdAsync(doctorId);
            var dayOff = days.FirstOrDefault(d => d.Id == id);
            if (dayOff == null)
                return NotFound();
            await _doctorDayOffRepo.DeleteAsync(dayOff);
            return Ok("Day off removed successfully");
        }
    }
}
