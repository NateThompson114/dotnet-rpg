namespace dotnet_rpg_web_api.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static List<Character> _characters = new()
    {
        new Character(),
        new Character() { Id = 1, Name = "Sam"}
    };

    public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<Character>>
        {
            Data = _characters
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<Character?>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<Character?>
        {
            Data = _characters.FirstOrDefault(c => c.Id == id)
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)
    {
        _characters.Add(newCharacter);

        var serviceResponse = new ServiceResponse<List<Character>>
        {
            Data = _characters
        };

        return serviceResponse;
    }
}