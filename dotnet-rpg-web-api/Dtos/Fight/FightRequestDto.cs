namespace dotnet_rpg_web_api.Dtos.Fight;

public class FightRequestDto
{
    public List<int> CharacterIds { get; set; } = new List<int>();
}