using Microsoft.AspNetCore.Identity;
using Registation.Dto;
using Registation.Models;

namespace Registation.Services;

public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;

public UserService(UserManager<AppUser> userManager)
{
    _userManager = userManager;
}

public async Task<AppUser> RegisterAsync(RegisterDto dto)
{
    var user = new AppUser
    {
        UserName = dto.Email,
        Email = dto.Email,
        PhoneNumber = dto.Mobile,
        CustomerName = dto.CustomerName,
        ICTNumber = dto.ICTNumber
    };

    var result = await _userManager.CreateAsync(user);
    return result.Succeeded ? user : null;
}

public async Task<bool> SetPinAsync(PinDto dto)
{
    if (dto.Pin != dto.ConfirmPin) return false;

    var user = await _userManager.FindByIdAsync(dto.UserId);
    if (user == null) return false;

    user.PIN = dto.Pin; // TODO: Hash in real apps
    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
}

public async Task<bool> SetBiometricAsync(string userId, bool enable)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) return false;

    user.BiometricEnabled = enable;
    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
}

public async Task<bool> ChangeEmailAsync(string userId, string newEmail)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) return false;

    user.Email = newEmail;
    user.UserName = newEmail;
    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
}
    }