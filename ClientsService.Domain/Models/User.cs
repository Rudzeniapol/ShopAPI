using ClientsService.Domain.SoftDelete;

namespace ClientsService.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public string Email { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    
    //Do I even need this field below?
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public bool IsActivated { get; set; }
}