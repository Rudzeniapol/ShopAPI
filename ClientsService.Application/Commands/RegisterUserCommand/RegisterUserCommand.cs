using ClientsService.Application.DTOs;
using ClientsService.Domain.Models;
using MediatR;

namespace ClientsService.Application.Commands.AddUserCommand;

public class RegisterUserCommand : IRequest<TokenDTO>
{
    public RegisterUserDTO RegisterUser { get; set; }
}