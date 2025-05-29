using Registation.Models;

namespace Registation.Services;

public interface IAuthService
{

    Task<AppUser> GetUserByICTAsync(string ictNumber);
}
