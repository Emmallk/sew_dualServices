using RegistrationService;

namespace PlayerStatisticsService;

public class Duel
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    
    public DateTime LastDuel { get; set; }
    
    public int DuelResult { get; set; }
}