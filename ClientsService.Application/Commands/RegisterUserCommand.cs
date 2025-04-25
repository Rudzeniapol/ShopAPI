using ClientsService.Application.DTOs;
using ClientsService.Domain.Models;
using MediatR;

namespace ClientsService.Application.Commands;

public class RegisterUserCommand : IRequest<TokenDTO>
{
    public RegisterUserDTO RegisterUser { get; set; }
}