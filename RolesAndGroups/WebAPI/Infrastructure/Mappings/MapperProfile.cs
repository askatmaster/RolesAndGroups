using AutoMapper;
using Models.Domain;
using Models.Dto;
namespace Infrastructure.Mappings;

/// <summary>
/// Профиль маппинга (Automapper)
/// </summary>
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Group, GroupDto>().ReverseMap();
    }
}