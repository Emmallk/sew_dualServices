namespace RegistrationService.Class;

public class Player
{
    public int Id { get; set; }
    public int Elo { get; set; }
    public DateTime? LastPlayedDuel { get; set; }
}