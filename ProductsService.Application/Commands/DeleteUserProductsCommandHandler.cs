using MediatR;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands;

public class DeleteUserProductsCommandHandler : IRequestHandler<DeleteUserProductsCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteUserProductsCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteUserProductsCommand request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsByUserIdAsync(request.UserId, cancellationToken);
        if (!products.Any())
        {
            throw new NotFoundException("You do not have any products.");
        }
        await _productRepository.DeleteRange(products, cancellationToken);
    }
}