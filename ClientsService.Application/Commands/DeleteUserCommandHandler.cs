using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (userToDelete == null)
        {
            return;
            //implement exception
        }
        await _userRepository.DeleteAsync(userToDelete, cancellationToken);
    }
}