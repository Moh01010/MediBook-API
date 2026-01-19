using MediBook.Dtos;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientsController(IPatientRepository patientRepo, UserManager<ApplicationUser> userManager)
        {
            _patientRepo = patientRepo;
            _userManager = userManager;
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest();
            var patient = await _patientRepo.GetByUserIdAsync(userId);
            if(patient == null)
            {
                patient = new Patient
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _patientRepo.CreateAsync(patient);
            }
            return Ok(new PatientProfileDto
            {
                Id = patient.Id,
                FullName = patient.User.FullName,
                Email = patient.User.Email,
                CreatedAt = patient.CreatedAt
            });
        }
        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpdateProfile(UpdatePatientProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient=await _patientRepo.GetByUserIdAsync(userId);
            if (patient == null) 
                return NotFound();
            patient.User.FullName=dto.FullName;
            await _patientRepo.SaveAsync();
            return Ok("Profile updated successfully");
        }
    }
}
