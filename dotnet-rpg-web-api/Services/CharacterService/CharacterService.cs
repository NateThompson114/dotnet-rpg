using AutoMapper;

namespace dotnet_rpg_web_api.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters()
    {
        var characters = await _context.Characters.ToListAsync();
        
        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = characters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToList()
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto?>> GetCharacterById(int id)
    {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

        var serviceResponse = new ServiceResponse<GetCharacterResponseDto?>
        {
            Data = _mapper.Map<GetCharacterResponseDto>(character)
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = await _context.Characters.Select(c=> _mapper.Map<GetCharacterResponseDto>(c)).ToListAsync()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();

        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (character == null)
                throw new NullReferenceException($"Character with Id '{updatedCharacter.Id}' not found");

            _mapper.Map(updatedCharacter, character);

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterResponseDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> DeleteCharacter(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>();

        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
                throw new NullReferenceException($"Character with Id '{id}' not found");

            _context.Characters.Remove(character);
            
            await _context.SaveChangesAsync();

            serviceResponse.Data = _context.Characters.AsQueryable().Select(c => _mapper.Map<GetCharacterResponseDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }
}