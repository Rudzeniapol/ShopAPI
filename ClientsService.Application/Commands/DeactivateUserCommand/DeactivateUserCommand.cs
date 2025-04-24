using System.Windows.Input;
using MediatR;

namespace ClientsService.Application.Commands.DeactivateUserCommand;

public class DeactivateUserCommand : IRequest
{
    public string email { get; set; }
}