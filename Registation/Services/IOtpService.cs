namespace Registation.Servicesl;

public interface IOtpService
{
    Task<string> GenerateAndSendOtp(string userId, string type);
    Task<bool> VerifyOtp(string userId, string code, string type);
}
