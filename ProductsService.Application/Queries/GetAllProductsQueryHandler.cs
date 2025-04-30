using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}