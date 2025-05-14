using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Commands;

public class RefreshTokenCommand : IRequest<TokenDTO>
{
    public string RefreshToken { get; set; }
}