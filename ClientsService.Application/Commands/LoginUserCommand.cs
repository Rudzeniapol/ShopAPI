using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Commands;

public class LoginUserCommand : IRequest<TokenDTO>
{
    public LoginUserDTO LoginUser { get; set; }
}