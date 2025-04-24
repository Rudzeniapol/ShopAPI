using ClientsService.Application.Commands.UpdateUserCommand;
using ClientsService.Application.DTOs;
using ClientsService.Application.Queries.GetUserByEmailQuery;
using ClientsService.Application.Queries.GetUsersQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientsService.API.Controllers;

[ApiController]
[Authorize]
[Route("api/clients/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }   
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegisterUserDTO>>> GetUsers(CancellationToken cancellationToken)
    {
        GetUsersQuery query = new GetUsersQuery();
        var users = await _mediator.Send(query, cancellationToken);
        return Ok(users);
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult<UserDTO>> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        GetUserByEmailQuery query = new GetUserByEmailQuery()
        {
            Email = email
        };
        var user = await _mediator.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpPut("{email}")]
    public async Task<ActionResult> UpdateUser(UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}