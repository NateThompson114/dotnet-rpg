using AutoMapper;
using dotnet_rpg_web_api.Dtos.Weapon;

namespace dotnet_rpg_web_api;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterResponseDto>();
        CreateMap<AddCharacterRequestDto, Character>();
        CreateMap<UpdateCharacterRequestDto, Character>();
        CreateMap<Weapon, GetWeaponResponseDto>();
    }
}