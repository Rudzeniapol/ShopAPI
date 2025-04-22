using System.Security.Claims;
using ClientsService.Application.DTOs;
using ClientsService.Domain.Models;

namespace ClientsService.Application.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDTO> GenerateJwtToken(User user, bool populateExp, CancellationToken cancellationToken = default);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, CancellationToken cancellationToken = default);
}