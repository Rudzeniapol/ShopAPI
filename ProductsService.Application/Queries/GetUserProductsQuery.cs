using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries;

public class GetUserProductsQuery : IRequest<IEnumerable<ProductDTO>>
{
    public int UserId { get; set; }
}