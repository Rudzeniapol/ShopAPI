using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Commands;

public class UpdateUserCommand : IRequest
{
    public string Email { get; init; } = string.Empty;
    public string NewUserName { get; init; } = string.Empty;
}