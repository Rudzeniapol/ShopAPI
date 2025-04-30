using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using MediatR;

namespace ClientsService.Application.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenDTO>
{
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RegisterUserCommandHandler(IPasswordService passwordService, IUserRepository userRepository, ITokenService tokenService)
    {
        _passwordService = passwordService;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.RegisterUser.Email);
        if (user != null)
        {
            throw new EntityExistsException("User with this email already exists");
        }
        var hashedPassword = _passwordService.HashPassword(request.RegisterUser.Password);
        var newUser = new User
        {
            Email = request.RegisterUser.Email,
            Username = request.RegisterUser.UserName,
            PasswordHash = hashedPassword,
            Role = request.RegisterUser.Role,
            IsActivated = true
        };
        await _userRepository.AddAsync(newUser, cancellationToken);
        return await _tokenService.GenerateJwtToken(newUser, false, cancellationToken);
    }
}