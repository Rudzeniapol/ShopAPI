using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Queries;

public class GetUserByEmailQuery : IRequest<UserDTO>
{
    public string Email { get; init; } = string.Empty;
}