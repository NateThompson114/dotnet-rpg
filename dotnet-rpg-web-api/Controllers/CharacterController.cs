using dotnet_rpg_web_api.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg_web_api.Controllers;

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
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> GetCharacter(int id)
    {
        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> AddCharacter([FromBody] AddCharacterRequestDto character)
    {
        return Ok(await _characterService.AddCharacter(character));
    }

    [HttpPut()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> UpdateCharacter([FromBody] UpdateCharacterRequestDto character)
    {
        var result = await _characterService.UpdateCharacter(character);
        return result.Success ? Ok(result) : NotFound(result);
    }
}