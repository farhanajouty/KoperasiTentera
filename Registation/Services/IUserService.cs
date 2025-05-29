using Registation.Dto;
using Registation.Models;

namespace Registation.Services;

public interface IUserService
{

    Task<AppUser> RegisterAsync(RegisterDto dto);
    Task<bool> SetPinAsync(PinDto dto);
    Task<bool> SetBiometricAsync(string userId, bool enable);
    Task<bool> ChangeEmailAsync(string userId, string newEmail);
}
