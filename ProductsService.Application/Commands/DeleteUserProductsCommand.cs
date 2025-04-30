using MediatR;

namespace ProductService.Application.Commands;

public class DeleteUserProductsCommand : IRequest
{
    public int UserId { get; set; }
}