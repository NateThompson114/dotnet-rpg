using AutoMapper;
using dotnet_rpg_web_api.Dtos.Fight;

namespace dotnet_rpg_web_api.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public FightService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

            var opponent = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

            if (attacker is null || opponent is null || attacker.Weapon is null)
            {
                response.Success = false;
                response.Message = "Something is wrong";

                return response;
            }

            var damage = DoWeaponAttackDamage(attacker, opponent);

            if (opponent.Hitpoints <= 0)
            {
                response.Message = $"{opponent.Name} has been defeated ({attacker.Name}[HP: {attacker.Hitpoints}] :: {opponent.Name}[HP: {opponent.Hitpoints}])! Both fighters are now healed up";
                opponent.Hitpoints = opponent.MaxHitpoints;
                attacker.Hitpoints = attacker.MaxHitpoints;
            }

            await _context.SaveChangesAsync();

            response.Data = new AttackResultDto
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHp = attacker.Hitpoints,
                OpponentHp = opponent.Hitpoints,
                Damage = damage
            };
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.InnerException?.Message ?? ex.Message;
        }

        return response;
    }

    private static int DoWeaponAttackDamage(Character attacker, Character opponent)
    {
        if (attacker.Weapon is null)
            throw new Exception("Attacker has no weapon");

        var damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
        damage -= new Random().Next(opponent.Defense);

        if (damage > 0) opponent.Hitpoints -= damage;
        return damage;
    }

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

            var opponent = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

            if (attacker is null || opponent is null || attacker.Skills is null)
            {
                response.Success = false;
                response.Message = "Something is wrong";

                return response;
            }

            var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

            if (skill is null)
            {
                response.Success = false;
                response.Message = $"{attacker.Name} doesn't know that skill!";
                return response;
            }

            var damage = DoSkillDamage(skill, attacker, opponent);

            if (opponent.Hitpoints <= 0)
            {
                response.Message = $"{opponent.Name} has been defeated ({attacker.Name}[HP: {attacker.Hitpoints}] :: {opponent.Name}[HP: {opponent.Hitpoints}])! Both fighters are now healed up";
                opponent.Hitpoints = opponent.MaxHitpoints;
                attacker.Hitpoints = attacker.MaxHitpoints;
            }

            await _context.SaveChangesAsync();

            response.Data = new AttackResultDto
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHp = attacker.Hitpoints,
                OpponentHp = opponent.Hitpoints,
                Damage = damage
            };
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.InnerException?.Message ?? ex.Message;
        }

        return response;
    }

    private static int DoSkillDamage(Skill skill, Character attacker, Character opponent)
    {
        var damage = skill.Damage + (new Random().Next(attacker.Intelligence));
        damage -= new Random().Next(opponent.Defense);

        if (attacker.Id == opponent.Id && skill.Id == 3)
        {
            attacker.Hitpoints -= damage;
            if (attacker.Hitpoints > attacker.MaxHitpoints) attacker.Hitpoints = attacker.MaxHitpoints;
        }
        else if (damage > 0) 
        {
            opponent.Hitpoints -= damage;
        }

        return damage;
    }

    public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
    {
        var response = new ServiceResponse<FightResultDto>()
        {
            Data = new FightResultDto()
        };

        try
        {
            var characters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharacterIds.Contains(c.Id))
                .ToListAsync();
            var defeated = false;
            while (!defeated)
            {
                foreach (var attacker in characters)
                {
                    var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                    var opponent = opponents[new Random().Next(opponents.Count)];

                    var damage = 0;
                    var attackUsed = string.Empty;
                    var isHeal = false;

                    var useWeapon = new Random().Next(2) == 0;
                    if (useWeapon && attacker.Weapon is not null)
                    {
                        attackUsed = attacker.Weapon.Name;
                        damage = DoWeaponAttackDamage(attacker, opponent);
                    }
                    else if(!useWeapon && attacker.Skills is not null)
                    {
                        var canHeal = attacker.Skills.Any(s => s.Id == 3);
                        var skills = new List<Skill>();

                        if (canHeal && attacker.Hitpoints <= attacker.MaxHitpoints*.75)
                        {
                            skills = attacker.Skills;
                        }
                        else
                        {
                            skills = attacker.Skills.Where(s => s.Id != 3).ToList();
                            if (!skills.Any()) skills = attacker.Skills.ToList();
                        }


                        var skill = skills[new Random().Next(skills.Count)];
                        attackUsed = skill.Name;
                        if(skill.Id == 3) isHeal = true;
                        damage = DoSkillDamage(skill, attacker, (skill.Id == 3 ? attacker : opponent));
                    }
                    else
                    {
                        response.Data.Log
                            .Add($"{attacker.Name} wasn't able to attack!");
                        continue;
                    }

                    response.Data.Log.Add(
                        $"{attacker.Name} {(isHeal ? "targets self " : $"attacks {opponent.Name}")} with {attackUsed} for {Math.Abs(damage)} points.");

                    if (opponent.Hitpoints <= 0)
                    {
                        defeated = true;
                        attacker.Victories++;
                        opponent.Defeats++;

                        response.Data.Log.Add($"{opponent.Name} has been defeated!");
                        response.Data.Log.Add($"{attacker.Name} wins with {attacker.Hitpoints} HP left!");
                        break;
                    }
                }
            }

            characters.ForEach(c =>
            {
                c.Fights++;
                c.Hitpoints = c.MaxHitpoints;
            });

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.InnerException?.Message ?? ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
    {
        var characters = await _context.Characters
            .Where(c => c.Fights > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats)
            .ToListAsync();

        var response = new ServiceResponse<List<HighScoreDto>>
        {
            Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
        };

        return response;
    }
}