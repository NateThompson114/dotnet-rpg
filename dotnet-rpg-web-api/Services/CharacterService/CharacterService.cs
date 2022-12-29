using System.Security.Claims;
using AutoMapper;

namespace dotnet_rpg_web_api.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters()
    {
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User!.Id == GetUserId()).ToListAsync();
        
        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = characters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToList()
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto?>> GetCharacterById(int id)
    {
        var character = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

        var serviceResponse = new ServiceResponse<GetCharacterResponseDto?>
        {
            Data = _mapper.Map<GetCharacterResponseDto>(character)
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>
        {
            Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c=> _mapper.Map<GetCharacterResponseDto>(c)).ToListAsync()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();

        try
        {
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User!.Id == GetUserId());
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
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

            if (character == null)
                throw new NullReferenceException($"Character with Id '{id}' not found");

            _context.Characters.Remove(character);
            
            await _context.SaveChangesAsync();

            serviceResponse.Data = 
                _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .AsQueryable().Select(c => _mapper.Map<GetCharacterResponseDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();

        try
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId());
            if (character == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";

                return serviceResponse;
            }

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
            if (skill == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Skill not found";

                return serviceResponse;
            }

            if (character.Skills!.Contains(skill))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character already has skill {skill.Name}";

                return serviceResponse;
            }

            character.Skills!.Add(skill);
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
}