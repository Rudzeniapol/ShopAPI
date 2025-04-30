using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Commands;

public class ReturnUserProductsCommandHandler : IRequestHandler<ReturnUserProductsCommand>
{
    private readonly IProductRepository _productRepository;

    public ReturnUserProductsCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ReturnUserProductsCommand request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetUserProductsWithoutFiltersAsync(request.UserId, cancellationToken);
        foreach (var product in products)
        {
            product.IsDeleted = false;
            product.DeletedAt = null;
        }
        await _productRepository.UpdateRange(products, cancellationToken);
    }
}