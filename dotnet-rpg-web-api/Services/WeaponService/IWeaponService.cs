using dotnet_rpg_web_api.Dtos.Weapon;

namespace dotnet_rpg_web_api.Services.WeaponService;

public interface IWeaponService
{
    Task<ServiceResponse<GetCharacterResponseDto>> AddWeapon(AddWeaponRequestDto neWeapon);
}