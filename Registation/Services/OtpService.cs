using Microsoft.EntityFrameworkCore;
using Registation.Data;
using Registation.Models;
using Registation.Servicesl;

namespace Registation.Services;

public class OtpService: IOtpService
{
        private readonly AppDbContext _context;

public OtpService(AppDbContext context)
{
    _context = context;
}

public async Task<string> GenerateAndSendOtp(string userId, string type)
{
    var code = new Random().Next(1000, 9999).ToString();

    var otp = new OtpCode
    {
        UserId = userId,
        Code = code,
        Type = type,
        Expiry = DateTime.UtcNow.AddMinutes(5),
        Used = false
    };

    _context.OtpCodes.Add(otp);
    await _context.SaveChangesAsync();

    // Replace with actual SMS/Email sending
    Console.WriteLine($"[Mock] OTP for {type}: {code}");

    return code;
}

public async Task<bool> VerifyOtp(string userId, string code, string type)
{
    var otp = await _context.OtpCodes.FirstOrDefaultAsync(o =>
        o.UserId == userId && o.Code == code && o.Type == type && !o.Used && o.Expiry > DateTime.UtcNow);

    if (otp == null) return false;

    otp.Used = true;
    await _context.SaveChangesAsync();

    return true;
}
    }