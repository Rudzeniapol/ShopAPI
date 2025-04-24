using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands.ActivateUserCommand;

public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActivateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.email, cancellationToken);
        if (user == null)
        {
            //implement exception
            return;
        }
        user.IsActivated = true;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}