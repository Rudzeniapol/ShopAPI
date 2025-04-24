using MediatR;

namespace ClientsService.Application.Commands.ActivateUserCommand;

public class ActivateUserCommand : IRequest
{
    public string email { get; set; }
}