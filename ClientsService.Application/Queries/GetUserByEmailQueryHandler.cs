using AutoMapper;
using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using MediatR;

namespace ClientsService.Application.Queries;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User with this email does not exist");
        }
        return _mapper.Map<UserDTO>(user);
    }
}