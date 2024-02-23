namespace PlayerStatisticsService;

public class PlayerStatistics
{
    
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public int CurrentEloRating { get; set; }
    public int NumberOfDuelsWon { get; set; }
    public int NumberOfDuelsLost { get; set; }
    public int NumberOfDuelsDraw { get; set; }
    public int NumberOfDuelsPlayed { get; set; }
    
    // Welche Zeit ???
    public int AverageDuelDuration { get; set; }
    public DateTime? LastDuelPlayedAt { get; set; }
}