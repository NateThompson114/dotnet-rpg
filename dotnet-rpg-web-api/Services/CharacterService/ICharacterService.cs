namespace dotnet_rpg_web_api.Services.CharacterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<Character>>> GetAllCharacters();
    Task<ServiceResponse<Character?>> GetCharacterById(int id);
    Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);
}