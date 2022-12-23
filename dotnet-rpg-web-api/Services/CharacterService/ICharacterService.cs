namespace dotnet_rpg_web_api.Services.CharacterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters();
    Task<ServiceResponse<GetCharacterResponseDto?>> GetCharacterById(int id);
    Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter);
    Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updatedCharacter);
    Task<ServiceResponse<bool>> DeleteCharacter(int id);

}