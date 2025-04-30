using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        user.Username = request.NewUserName;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}