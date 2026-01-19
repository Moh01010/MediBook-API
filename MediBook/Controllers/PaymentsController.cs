using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediBook.Interface;
using MediBook.Dtos;
using System.Threading.Tasks;
using MediBook.Enum;
using MediBook.Models;
namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly INotificationService _notificationService;

        public PaymentsController(IPaymentRepository paymentRepo, IAppointmentRepository appointmentRepo, INotificationService notificationService)
        {
            _paymentRepo = paymentRepo;
            _appointmentRepo = appointmentRepo;
            _notificationService = notificationService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var appointment=await _appointmentRepo.GetByIdAsync(dto.AppointmentId);
            if (appointment == null)
                return NotFound("Appointment not found");

            if (appointment == null)
                return NotFound("Appointment not found");

            var payment = new Payment
            {
                AppointmentId = appointment.Id,
                Amount = dto.Amount,
                Status = PaymentStatus.Pending
            };

            await _paymentRepo.CreateAsync(payment);

            return Ok("Payment created successfully");
        }
        [HttpPost("{appointmentId}/confirm")]
        public async Task<IActionResult> Confirm(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null)
                return NotFound("Appointment not found");

            if (appointment.Payment == null)
                return BadRequest("No payment found");

            appointment.Payment.Status = PaymentStatus.Paid;
            appointment.Status = AppointmentStatus.Confirmed;

            await _appointmentRepo.SaveAsync();

            await _notificationService.NotifyAsync(
             appointment.Patient.UserId,
             "Payment Confirmed",
              "Your appointment has been confirmed successfully.");

            return Ok("Payment confirmed and appointment confirmed");
        }
        [HttpPost("{appointmentId}/fail")]
        public async Task<IActionResult> Fail(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null)
                return NotFound("Appointment not found");

            if (appointment.Payment == null)
                return BadRequest("No payment found");

            appointment.Payment.Status = PaymentStatus.Failed;

            await _appointmentRepo.SaveAsync();

            return Ok("Payment failed");
        }
    }
}
