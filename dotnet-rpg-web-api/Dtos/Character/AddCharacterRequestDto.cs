using dotnet_rpg_web_api.Statics;

namespace dotnet_rpg_web_api.Dtos.Character;

public class AddCharacterRequestDto
{
    public string Name { get; set; } = "Frodo";
    public int Hitpoints { get; set; } = 100;
    public int MaxHitpoints { get; set; } = 100;
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public RpgClass Class { get; set; } = RpgClass.Knight;
    public int Fights { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }
}