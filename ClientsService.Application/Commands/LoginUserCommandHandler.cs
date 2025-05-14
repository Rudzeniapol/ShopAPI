using System.Security.Authentication;
using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }

    public async Task<TokenDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.LoginUser.Email, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("Invalid login information");
        }
        
        if (!_passwordService.VerifyPassword(user.PasswordHash, request.LoginUser.Password))
        {
            throw new BadRequestException("Invalid login information");
        }
        
        return await _tokenService.GenerateJwtToken(user, true, cancellationToken);
    }
}