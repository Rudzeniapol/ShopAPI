using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Models;

namespace ProductService.Application.Queries;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
{
}