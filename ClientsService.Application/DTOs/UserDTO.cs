namespace ClientsService.Application.DTOs;

public record UserDTO
{
    public int Id { get; set; }
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}