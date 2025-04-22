namespace ClientsService.Application.DTOs;

public record LoginUserDTO
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}