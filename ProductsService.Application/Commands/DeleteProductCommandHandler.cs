using MediatR;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByNameAsync(request.ProductName, request.UserId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException("Product with this name does not exist.");
        }
        await _productRepository.DeleteAsync(product, cancellationToken);
    }
}