using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Application.Queries;
using ClientsService.Application.Services;
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
    
    [Authorize(Policy = "AllUsers")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegisterUserDTO>>> GetUsers(CancellationToken cancellationToken)
    {
        GetUsersQuery query = new GetUsersQuery();
        var users = await _mediator.Send(query, cancellationToken);
        return Ok(users);
    }
    
    [Authorize(Policy = "AllUsers")]
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

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{email}")]
    public async Task<ActionResult> UpdateUser(string email, string newUserName,
        CancellationToken cancellationToken)
    {
        UpdateUserCommand command = new UpdateUserCommand()
        {
            Email = email,
            NewUserName = newUserName
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{email}")]
    public async Task<ActionResult> DeleteUser(string email, CancellationToken cancellationToken)
    {
        DeleteUserCommand command = new DeleteUserCommand()
        {
            Email = email
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{email}/deactivation")]
    public async Task<ActionResult> DeactivateUser(string email, CancellationToken cancellationToken)
    {
        DeactivateUserCommand command = new DeactivateUserCommand()
        {
            Email = email
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{email}/activation")]
    public async Task<ActionResult> ActivateUser(string email, CancellationToken cancellationToken)
    {
        ActivateUserCommand command = new ActivateUserCommand()
        {
            Email = email
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "AllUsers")]
    [HttpGet("guid-demo")]
    public ActionResult<string> GetGuidDemo()
    {
        // Демонстрация использования Singleton
        var guid = GuidGeneratorSingleton.Instance.GenerateGuid();
        return Ok($"Generated GUID from Singleton: {guid}");
    }
}