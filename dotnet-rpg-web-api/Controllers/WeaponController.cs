using dotnet_rpg_web_api.Dtos.Weapon;
using dotnet_rpg_web_api.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg_web_api.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
[Authorize]
public class WeaponController : ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> Add(AddWeaponRequestDto newWeapon)
    {
        return Ok(await _weaponService.AddWeapon(newWeapon));
    }
}