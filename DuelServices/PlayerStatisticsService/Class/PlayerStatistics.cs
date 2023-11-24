namespace PlayerStatisticsService.Class;

public class PlayerStatistics
{
    public int PlayerId { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    
    public string PlayerName { get; set; }
    public object NumberOfDuelsWon { get; set; }
    public object NumberOfDuelsLost { get; set; }
    public object NumberOfDuelsDraw { get; set; }
    public object NumberOfDuelsPlayed { get; set; }
    public object AverageDuelDuration { get; set; }
    public object LastDuelPlayedAt { get; set; }
}