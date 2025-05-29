using Microsoft.AspNetCore.Identity;

namespace Registation.Models;

public class AppUser: IdentityUser
{
    public string CustomerName { get; set; }
    public string ICTNumber { get; set; }
    public string PIN { get; set; } 
    public bool BiometricEnabled { get; set; }
}
