using AutoMapper;

namespace dotnet_rpg_web_api.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    private static List<Character> _characters = new()
    {
        new Character(),
        new Character() { Id = 1, Name = "Sam"}
    };

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters()
    {
        var characters = _characters;
        
        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = _characters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToList()
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto?>> GetCharacterById(int id)
    {
        var character = _characters.FirstOrDefault(c => c.Id == id);

        var serviceResponse = new ServiceResponse<GetCharacterResponseDto?>
        {
            Data = _mapper.Map<GetCharacterResponseDto>(character)
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.Id = _characters.Max(c => c.Id) + 1;
        _characters.Add(character);

        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = _characters.Select(c=> _mapper.Map<GetCharacterResponseDto>(c)).ToList()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();

        try
        {
            var character = _characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
            if (character == null)
                throw new NullReferenceException($"Character with Id '{updatedCharacter.Id}' not found");

            _mapper.Map(updatedCharacter, character);

            //character.Name = updatedCharacter.Name;
            //character.Hitpoints = updatedCharacter.Hitpoints;
            //character.Strength = updatedCharacter.Strength;
            //character.Defense = updatedCharacter.Defense;
            //character.Intelligence = updatedCharacter.Intelligence;
            //character.Class = updatedCharacter.Class;

            serviceResponse.Data = _mapper.Map<GetCharacterResponseDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }
}