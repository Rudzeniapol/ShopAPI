using System.Collections.Immutable;
using ClientsService.Application.DTOs;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Queries.GetUsersQuery;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDTO>> Handle(Queries.GetUsersQuery.GetUsersQuery request, CancellationToken cancellationToken)
    {
        //implement this
        return ImmutableArray<UserDTO>.Empty;
    }
}