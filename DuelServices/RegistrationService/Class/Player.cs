namespace RegistrationService.Class;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double EloRating { get; set; }
    
    public DateTime? LastPlayedDuel { get; set; }
}