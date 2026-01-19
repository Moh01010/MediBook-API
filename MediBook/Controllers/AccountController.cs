using MediBook.Dtos;
using MediBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace MediBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, EmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName=dto.FullName,
                    Email = dto.Email,
                    UserName=dto.Email
                };
                var result= await _userManager.CreateAsync(user,dto.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                await _userManager.AddToRoleAsync(user, "Patient");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/account/confirm-email?userId={user.Id}&token={token}";

                await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                    $"<h3>Please confirm your account</h3><a href='{confirmationLink}'>Click here</a>");
                return Ok("Account Registered Successfully. Please check your email to confirm.");
            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user=await _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
                return Unauthorized("Invalid email or password");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Please confirm your email first.");

            var valid=await _userManager.CheckPasswordAsync(user,dto.Password);
            if(!valid)
                return Unauthorized("Invalid email or password");

            var roles=await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim (ClaimTypes.Email,user.Email),
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role,role));
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(
                 double.Parse(_configuration["Jwt:DurationInMinutes"])),
                 signingCredentials: new SigningCredentials(
                 key, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            });
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid user");

            var decodedToken =
                Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return BadRequest("Email confirmation failed");

            return Ok("Email confirmed successfully");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Ok("If email exists, password reset link has been sent");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetLink =
            $"{Request.Scheme}://{Request.Host}/api/account/reset-password" +
            $"?userId={user.Id}&token={token}";
            await _emailService.SendEmailAsync(
            user.Email,
            "Reset your password",
            $@"
            <h3>Reset Password</h3>
            <p>Click the link below to reset your password:</p>
            <a href='{resetLink}'>Reset Password</a>");

            return Ok("Password reset link sent to your email.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return BadRequest("Invalid user");
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                dto.NewPassword
            );
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("Password has been reset successfully");
        }
    }
}
