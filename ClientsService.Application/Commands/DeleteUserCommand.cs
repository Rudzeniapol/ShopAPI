using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Commands;

public class DeleteUserCommand : IRequest
{
    public string Email { get; set; }
}