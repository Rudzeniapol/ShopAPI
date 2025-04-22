using ClientsService.Application.Commands.AddUserCommand;
using ClientsService.Application.DTOs;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands.LoginUserCommand;

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
        var user = await _userRepository.GetUserByEmailAsync(request.User.Email, cancellationToken);    
        if (user == null || !_passwordService.VerifyPassword(user.PasswordHash,request.User.Password))
            throw new Exception($"Пользователь с почтой {request.User.Email} не найден"); 
            //Implement exception above (or change code)
        return await _tokenService.GenerateJwtToken(user, true, cancellationToken);
    }
}