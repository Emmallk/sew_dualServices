namespace PlayerStatisticsService;

public class DuelOutcome
{
    public int Player1Id { get; set; }
    public string Player1Name { get; set; }
    public int Player2Id { get; set; }
    public string Player2Name { get; set; }
    public DuelResult Result { get; set; }
}