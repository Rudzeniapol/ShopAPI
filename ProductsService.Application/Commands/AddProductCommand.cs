using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands;

public class AddProductCommand : IRequest
{
    public ProductDTO Product { get; set; }
    public int UserId { get; set; }
}