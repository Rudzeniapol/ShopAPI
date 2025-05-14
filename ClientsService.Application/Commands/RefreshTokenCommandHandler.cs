using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDTO>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    
    public async Task<TokenDTO> Handle(RefreshTokenCommand token, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(token.RefreshToken, cancellationToken);
        if (user == null || user.RefreshToken != token.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new BadRequestException("Invalid refresh token");
        }

        return await _tokenService.GenerateJwtToken(user, populateExp: false, cancellationToken);
    }
}