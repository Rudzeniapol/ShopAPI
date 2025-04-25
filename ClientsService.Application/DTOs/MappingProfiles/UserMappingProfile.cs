using AutoMapper;
using ClientsService.Domain.Models;

namespace ClientsService.Application.DTOs.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(s => s.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(s => s.Role))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.IsActivated, opt => opt.Ignore())
            .ReverseMap();
    }
}