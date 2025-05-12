using ClientsService.Application.Clients;
using ClientsService.Application.Clients.Interfaces;
using ClientsService.Application.Exceptions;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IProductServiceClient _productServiceClient;
    
    public ActivateUserCommandHandler(IUserRepository userRepository, IProductServiceClient productServiceClient)
    {
        _userRepository = userRepository;
        _productServiceClient = productServiceClient;
    }

    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User with this email does not exist");
        }
        await _productServiceClient.ReturnAsync(cancellationToken);
        user.IsActivated = true;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}