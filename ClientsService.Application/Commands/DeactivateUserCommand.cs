using System.Windows.Input;
using MediatR;

namespace ClientsService.Application.Commands;

public class DeactivateUserCommand : IRequest
{
    public string Email { get; set; }
}