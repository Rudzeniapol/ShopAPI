using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using ProductService.Application.Commands;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;

namespace ProductsService.API.Controllers;

[ApiController]
[Authorize]
[Route("api/products/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetUserId()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return null;

        if (!int.TryParse(userId, out var parsedUserId))
            return null;
        return parsedUserId;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(CancellationToken cancellationToken)
    {
        GetAllProductsQuery query = new GetAllProductsQuery();
        var products = await _mediator.Send(query, cancellationToken);
        return Ok(products);
    }

    [HttpGet("myProducts")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetMyProducts(CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        GetUserProductsQuery query = new GetUserProductsQuery()
        {
            UserId = uId,
        };
        var products = await _mediator.Send(query, cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] ProductDTO product, CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        AddProductCommand command = new AddProductCommand()
        {
            Product = product,
            UserId = uId,
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProduct([FromBody] ProductDTO product, CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        UpdateProductCommand command = new UpdateProductCommand()
        {
            Product = product,
            UserId = uId,
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{productName}")]
    public async Task<ActionResult> DeleteProduct(string productName, CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        DeleteProductCommand command = new DeleteProductCommand()
        {
            ProductName = productName,
            UserId = uId,
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("userProducts")]
    public async Task<ActionResult> DeleteAllUserProducts(CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        DeleteUserProductsCommand command = new DeleteUserProductsCommand()
        {
            UserId = uId,
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("activation")]
    public async Task<ActionResult> ReturnUserProducts(CancellationToken cancellationToken)
    {
        int? userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        int uId = userId.Value;
        ReturnUserProductsCommand command = new ReturnUserProductsCommand()
        {
            UserId = uId,
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}