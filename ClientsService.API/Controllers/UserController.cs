using ClientsService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientsService.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegisterUserDTO>>> GetUsers(CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpGet("{email}")]
    public async Task<ActionResult<RegisterUserDTO>> GetUser(string email, CancellationToken cancellationToken)
    {
        return Ok();
    }
}