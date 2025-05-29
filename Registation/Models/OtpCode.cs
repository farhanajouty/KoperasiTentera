namespace Registation.Models;

public class OtpCode
{

    public int Id { get; set; }
    public string UserId { get; set; }
    public string Code { get; set; }
    public string Type { get; set; } 
    public DateTime Expiry { get; set; }
    public bool Used { get; set; }
}
