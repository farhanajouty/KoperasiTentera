using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Registation.Models;

namespace Registation.Services;

public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;

public AuthService(UserManager<AppUser> userManager)
{
    _userManager = userManager;
}

public async Task<AppUser> GetUserByICTAsync(string ictNumber)
{
    return await _userManager.Users.FirstOrDefaultAsync(u => u.ICTNumber == ictNumber);
}
    }
