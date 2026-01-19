using MediBook.Dtos;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Threading.Tasks;

namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientRepository _petientRepo;

        public DoctorsController(IDoctorRepository doctorRepo, UserManager<ApplicationUser> userManager, IPatientRepository petientRepo)
        {
            _doctorRepo = doctorRepo;
            _userManager = userManager;
            _petientRepo = petientRepo;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateDoctorDto dto)
        {
            var user = await _userManager.FindByEmailAsync (dto.Email);
            if (user == null) 
                return BadRequest("User not found");
            var patient = await _petientRepo.GetByUserIdAsync(user.Id);
            if (patient != null)
            {
                _petientRepo.DeleteAsync(patient);
            }
            var doctor = new Doctor
            {
                UserId = user.Id,
                ClinicId = dto.ClinicId,
                Specialty = dto.Specialty,
                Bio = dto.Bio,
                SlotDuration = dto.SlotDuration
            };
           
            await _doctorRepo.CreateAsync(doctor);
            //await _userManager.AddToRoleAsync(user, "Doctor");
            if (!await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Patient");
                await _userManager.AddToRoleAsync(user, "Doctor");
            }
            return Ok("Doctor created successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors =await _doctorRepo.GetAllAsync();

            var result = doctors.Select(d=> new DoctorDto
            {
                Id = d.Id,
                FullName = d.User.FullName,
                Specialty = d.Specialty,
                SlotDuration = d.SlotDuration,
                ClinicName = d.Clinic.Name
            });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor == null) 
                return NotFound();
            var result = new DoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.User.FullName,
                Specialty = doctor.Specialty,
                SlotDuration = doctor.SlotDuration,
                ClinicName = doctor.Clinic.Name
            };
            return Ok(result);
        }
    }
}
