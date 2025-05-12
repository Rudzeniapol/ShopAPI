using ClientsService.Application.Clients;
using ClientsService.Application.Clients.Interfaces;
using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Commands;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IProductServiceClient _productServiceClient;

    public DeactivateUserCommandHandler(IUserRepository userRepository, IProductServiceClient productServiceClient)
    {
        _userRepository = userRepository;
        _productServiceClient = productServiceClient;
    }

    public async Task Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User with this email does not exist");
        }
        await _productServiceClient.DeleteAsync(cancellationToken);
        user.IsActivated = false;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}