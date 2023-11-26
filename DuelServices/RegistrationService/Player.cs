namespace RegistrationService;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EloRating { get; set; }
    public DateTime? LastPlayedDuel { get; set; }
}