using ClientsService.Application.DTOs;
using MediatR;

namespace ClientsService.Application.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDTO>>
{
}