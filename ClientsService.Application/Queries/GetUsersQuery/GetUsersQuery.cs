using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Queries.GetUsersQuery;

public class GetUsersQuery : IRequest<IEnumerable<UserDTO>>
{
}