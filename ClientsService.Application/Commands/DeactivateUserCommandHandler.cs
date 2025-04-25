using ClientsService.Application.DTOs;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeactivateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.email, cancellationToken);
        if (user == null)
        {
            //implement exception
            return;
        }
        user.IsActivated = false;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}