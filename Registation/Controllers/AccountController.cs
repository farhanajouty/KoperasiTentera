using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registation.Dto;
using Registation.Services;
using Registation.Servicesl;

namespace Registation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;
    private readonly IAuthService _authService;
    public AccountController(IUserService userService, IOtpService otpService, IAuthService authService)
        {
            _userService = userService;
            _otpService = otpService;
            _authService = authService;
        }

        // 1. Register new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _userService.RegisterAsync(dto);
            if (user == null)
                return BadRequest("User already exists or registration failed.");

            await _otpService.GenerateAndSendOtp(user.Id, "mobile");

            return Ok(new { userId = user.Id, message = "OTP sent to mobile." });
        }

        // 2. Verify OTP
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(OtpVerifyDto dto)
        {
            var valid = await _otpService.VerifyOtp(dto.UserId, dto.Code, dto.Type);
            if (!valid)
                return BadRequest("Invalid or expired OTP.");

            if (dto.Type == "mobile")
            {
                await _otpService.GenerateAndSendOtp(dto.UserId, "email");
                return Ok("Mobile verified. OTP sent to email.");
            }

            return Ok("Email verified.");
        }

        // 3. Set PIN
        [HttpPost("set-pin")]
        public async Task<IActionResult> SetPin(PinDto dto)
        {
            var result = await _userService.SetPinAsync(dto);
            return result ? Ok("PIN set successfully.") : BadRequest("PINs do not match.");
        }

        // 4. Enable/Disable Biometric
        [HttpPost("set-biometric")]
        public async Task<IActionResult> SetBiometric(BiometricDto dto)
        {
            var result = await _userService.SetBiometricAsync(dto.UserId, dto.Enable);
            return result ? Ok("Biometric preference updated.") : NotFound("User not found.");
        }

        // 5. Login via ICT Number
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.GetUserByICTAsync(dto.ICTNumber);
            if (user == null)
                return Unauthorized("Invalid ICT number.");

            await _otpService.GenerateAndSendOtp(user.Id, "mobile");
            return Ok(new { userId = user.Id, message = "OTP sent to mobile." });
        }

        // 6. Verify login email OTP
        [HttpPost("verify-login-otp")]
        public async Task<IActionResult> VerifyLoginOtp(OtpVerifyDto dto)
        {
            var valid = await _otpService.VerifyOtp(dto.UserId, dto.Code, dto.Type);
            if (!valid)
                return BadRequest("Invalid or expired OTP.");

            if (dto.Type == "mobile")
            {
                await _otpService.GenerateAndSendOtp(dto.UserId, "email");
                return Ok("Mobile verified. OTP sent to email.");
            }

            return Ok("Login successful. Email verified.");
        }

        // 7. Change email
        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail(EmailChangeDto dto)
        {
            var result = await _userService.ChangeEmailAsync(dto.UserId, dto.NewEmail);
            return result ? Ok("Email changed successfully.") : BadRequest("Email change failed.");
        }
    }