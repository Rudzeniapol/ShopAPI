using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries;

public class GetUserProductsQueryHandler : IRequestHandler<GetUserProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetUserProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsByUserIdAsync(request.UserId, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}