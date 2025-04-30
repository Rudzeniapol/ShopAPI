using AutoMapper;
using ProductService.Domain.Models;

namespace ProductService.Application.DTOs.MappingProfiles;

public class ProductMappingProfile : Profile
{

    public ProductMappingProfile()
    {
        CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(s => s.ProductName))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(s => s.IsAvailable))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(dest => dest.CountInStock, opt => opt.MapFrom(s => s.CountInStock))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ReverseMap();
    }
}