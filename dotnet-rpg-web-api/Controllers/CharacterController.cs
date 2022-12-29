using System.Security.Claims;
using dotnet_rpg_web_api.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg_web_api.Controllers;

[Authorize]
[Route("api/[controller]/[Action]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> GetAll()
    {
        return Ok(await _characterService.GetAllCharacters());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> Get(int id)
    {
        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> Add([FromBody] AddCharacterRequestDto character)
    {
        return Ok(await _characterService.AddCharacter(character));
    }

    [HttpPut()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> Update([FromBody] UpdateCharacterRequestDto character)
    {
        var result = await _characterService.UpdateCharacter(character);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> Delete(int id)
    {
        var result = await _characterService.DeleteCharacter(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> AddSkill([FromBody] AddCharacterSkillDto newCharacterSkill)
    {
        return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
    }
}