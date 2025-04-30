using MediatR;

namespace ProductService.Application.Commands;

public class ReturnUserProductsCommand : IRequest
{
    public int UserId { get; set; }
}