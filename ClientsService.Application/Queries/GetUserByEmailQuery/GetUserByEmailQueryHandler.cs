using ClientsService.Application.DTOs;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Queries.GetUserByEmailQuery;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        //map with mapper to UserDTO
        return null;
    }
}