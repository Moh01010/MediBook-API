using MediBook.Dtos;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicRepository _clinicRepo;

        public ClinicsController(IClinicRepository clinicRepo)
        {
            _clinicRepo = clinicRepo;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateClinicDto dto)
        {
            var clinic = new Clinic
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                CreatedAt = DateTime.UtcNow
            };
            await _clinicRepo.CreateAsync(clinic);
            return Ok("Clinic created successfully");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var clinics=await _clinicRepo.GetAllAsync();
            var result = clinics.Select(c => new ClinicDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone
            });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic=await _clinicRepo.GetByIdAsync(id);
            if (clinic==null)
                return NotFound();
            return Ok(new ClinicDto
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                Phone = clinic.Phone
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id,UpdateClinicDto dto)
        {
            var clinic = await _clinicRepo.GetByIdAsync(id);
            if(clinic==null)
                return BadRequest();
            clinic.Name = dto.Name;
            clinic.Address = dto.Address;
            clinic.Phone = dto.Phone;

            await _clinicRepo.SaveAsync();
            return Ok("Clinic updated successfully");
        }
    }
}
