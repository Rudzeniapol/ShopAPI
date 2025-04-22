using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Commands.LoginUserCommand;

public class LoginUserCommand : IRequest<TokenDTO>
{
    public LoginUserDTO User { get; set; }
}