using MediatR;

namespace ProductService.Application.Commands;

public class DeleteProductCommand : IRequest
{
    public string ProductName { get; set; }
    public int UserId { get; set; }
}