using RegistrationService;

namespace DuelSimulation;

public class Duel
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    
    public DateTime ScheduledTime { get; set; }
}