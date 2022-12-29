using dotnet_rpg_web_api.Dtos.Fight;

namespace dotnet_rpg_web_api.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
    Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
}