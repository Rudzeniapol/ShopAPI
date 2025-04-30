using AutoMapper;
using MediatR;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetProductByNameAsync(request.Product.ProductName, request.UserId, cancellationToken);
        if (existingProduct == null)
        {
            throw new NotFoundException("Product with this name does not exist.");
        }
        existingProduct.IsAvailable = request.Product.IsAvailable;
        existingProduct.Price = request.Product.Price;
        existingProduct.Description = request.Product.Description;
        existingProduct.CountInStock = request.Product.CountInStock;
        await _productRepository.UpdateAsync(existingProduct, cancellationToken);
    }
}