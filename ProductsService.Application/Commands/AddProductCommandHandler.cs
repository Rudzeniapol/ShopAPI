using AutoMapper;
using MediatR;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Commands;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetProductByNameAsync(request.Product.ProductName, request.UserId, cancellationToken);
        if (existingProduct != null)
        {
            throw new EntityExistsException("Product with this name already exists.");
        }
        Product product = _mapper.Map<Product>(request.Product);
        product.IsAvailable = true;
        product.CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow);
        product.UserId = request.UserId;
        await _productRepository.AddAsync(product, cancellationToken);
    }
}