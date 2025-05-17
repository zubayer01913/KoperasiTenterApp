using KoperasiTenterApp.Data;
using KoperasiTenterApp.Dtos;
using KoperasiTenterApp.Helpers;
using KoperasiTenterApp.Models;
using KoperasiTenterApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoperasiTenterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("check-user-by-ic")]
        public async Task<IActionResult> CheckUserByIC([FromBody] string icNumber)
        {
            var user = _context.Users.FirstOrDefault(u => u.ICNumber == icNumber);

            if (user == null)
               return Ok(new { userId = "", message = "User not found" });


            //Send mobile otp
            if(!user.IsMobileVerified)
                await SendOtp(new SendOtpInputDto() { MobileNumber = user.MobileNumber });


            var result = new UserOutputDto()
            {
                UserId = user.Id,
                MobileNumber = user.MobileNumber,
                EmailAddress = user.EmailAddress,
                IsEmailVerified = user.IsEmailVerified,
                IsMobileVerified = user.IsMobileVerified,
                Message = "Account already exists."
            };

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputDto dto)
        {

            var user = new User
            {
                CustomerName = dto.CustomerName,
                ICNumber = dto.ICNumber,
                MobileNumber = dto.MobileNumber,
                EmailAddress = dto.EmailAddress,
            };

             _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Send mobile otp
            await SendOtp(new SendOtpInputDto() { MobileNumber = dto.MobileNumber });

            var result = new UserOutputDto()
            {
                UserId = user.Id,
                MobileNumber = user.MobileNumber,
                EmailAddress = user.EmailAddress,
                IsEmailVerified = user.IsEmailVerified,
                IsMobileVerified = user.IsMobileVerified,
            };

            return Ok(result);

        }


        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpInputDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.MobileNumber == dto.MobileNumber || u.EmailAddress == dto.EmailAddress);
            if (user == null)
                return NotFound("User not found");

            var otp = AuthHelper.GenerateOtp();
            user.OtpCode = otp;
            user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync();

            // Simulate sending OTP - replace with actual SMS service


            var result = new SuccessOutputDto()
            {
                IsSuccess = true,
                UserId = user.Id,
                Message = $"OTP has been sent successfully. OTP is ({otp} for test purpose)" 
            };
            return Ok(result);
        }


        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpInputDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.MobileNumber == dto.MobileNumber || u.EmailAddress == dto.EmailAddress);
            if (user == null || user.OtpCode != dto.Code || user.OtpExpiresAt < DateTime.UtcNow)
                return BadRequest("Incorrect  or expired OTP");

            user.OtpCode = null;
            user.OtpExpiresAt = null;

            if (!dto.MobileNumber.IsNullOrEmpty())
                user.IsMobileVerified = true;

            if (!dto.EmailAddress.IsNullOrEmpty())
                user.IsEmailVerified = true;

            _context.SaveChanges();


            var result = new SuccessOutputDto()
            {
                IsSuccess = true,
                UserId = user.Id,
                Message = "OTP has been verified successfully"
            };
            return Ok(result);
        }


        [HttpPost("set-pin")]
        public IActionResult SetPin([FromBody] SetPinInputDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
            if (user == null || !user.IsMobileVerified)
                return BadRequest("User mobile not verified");

            if (user == null || !user.IsEmailVerified)
                return BadRequest("User email not verified");

            AuthHelper.CreatePinHash(dto.Pin, out var hash, out var salt);
            user.PinHash = hash;
            user.PinSalt = salt;
            user.AcceptedPrivacyPolicy = dto.AcceptedPrivacyPolicy;
            user.IsBiometricEnabled = dto.EnableBiometric;

            _context.SaveChanges();


            var result = new SuccessOutputDto()
            {
                IsSuccess = true,
                UserId = user.Id,
                Message = "PIN has been set successfully"
            };
            return Ok(result);
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInputDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.ICNumber == dto.ICNumber);
            if (user == null || !AuthHelper.VerifyPin(dto.Pin, user.PinHash, user.PinSalt))
                return Unauthorized("Invalid credentials");

             var token = _tokenService.CreateToken(user);
             return Ok(new AuthResponseOutDto { Token = token, BiometricEnabled = user.IsBiometricEnabled });
        }


        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("This is a protected endpoint.");
        }
    }
}
