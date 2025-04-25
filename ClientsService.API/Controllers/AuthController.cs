using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClientsService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginUserCommand command)
    {
        var tokens = await _mediator.Send(command);
        return Ok(tokens);
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenDTO>> Register([FromBody] RegisterUserCommand command)
    {
        var tokens = await _mediator.Send(command);
        return Ok(tokens);
    }
}