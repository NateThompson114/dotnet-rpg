namespace dotnet_rpg_web_api.Dtos.Weapon;

public class AddWeaponRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }
    public int CharacterId { get; set; }
}