using MediatR;

namespace ClientsService.Application.Commands;

public class ActivateUserCommand : IRequest
{
    public string email { get; set; }
}