namespace PlayerStatisticsService;

public class PlayerStatistics
{

    public int Wins { get; set; }
    public int Losses { get; set; }
    public object AverageDuelDuration { get; set; }
    
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    
    public int CurrentEloRating { get; set; }

    public int NumberOfDuelsPlayed { get; set; }
    public int NumberOfDuelsWon { get; set; }
    public int NumberOfDuelsLost { get; set; }
    public int NumberOfDuelsDraw { get; set; }
    
    public DateTime? LastDuelPlayedAt { get; set; }
}