using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RegistrationService;

namespace PlayerStatisticsService.Controllers;

[ApiController]
[Route("Statistics")]
public class PlayerStatisticsController : ControllerBase {
    //private readonly Dictionary<int, PlayerStatistics> playerStatistics = new Dictionary<int, PlayerStatistics>(); // Storing player statistics

    private List<PlayerStatistics> playerStatistics = new List<PlayerStatistics>();
    /*{
        new PlayerStatistics {
            PlayerId = 1,
            PlayerName = "Alice", 
            CurrentEloRating = 1500,
            LastDuelPlayedAt = DateTime.Parse("2023-11-26T19:46:14.262Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 2,
            PlayerName = "Bob",
            CurrentEloRating = 1600,
            LastDuelPlayedAt = DateTime.Parse("2023-11-25T18:30:00.000Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 3,
            PlayerName = "Charlie",
            CurrentEloRating = 1420,
            LastDuelPlayedAt = DateTime.Parse("2023-11-24T21:15:30.500Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 4,
            PlayerName = "David",
            CurrentEloRating = 1550,
            LastDuelPlayedAt = DateTime.Parse("2023-11-26T10:00:00.000Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 5,
            PlayerName = "Eva",
            CurrentEloRating = 1480,
            LastDuelPlayedAt = DateTime.Parse("2023-11-26T15:30:00.000Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 6,
            PlayerName = "Frank",
            CurrentEloRating = 1450,
            LastDuelPlayedAt = DateTime.Parse("2023-11-23T14:45:00.000Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 7,
            PlayerName = "Grace",
            CurrentEloRating = 1580,
            LastDuelPlayedAt = DateTime.Parse("2023-11-25T22:30:45.750Z"),
            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 8,
            PlayerName = "Hank",
            CurrentEloRating = 1520,
            LastDuelPlayedAt = DateTime.Parse("2023-11-26T12:30:00.000Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 9,
            PlayerName = "Ivy",
            CurrentEloRating = 1430,
            LastDuelPlayedAt = DateTime.Parse("2023-11-24T19:20:15.300Z"),

            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        },
        new PlayerStatistics {
            PlayerId = 10,
            PlayerName = "Jack",
            CurrentEloRating = 1475,
            LastDuelPlayedAt = DateTime.Parse("2023-11-26T14:00:00.000Z"),
            NumberOfDuelsPlayed = 0,
            NumberOfDuelsWon = 0,
            NumberOfDuelsLost = 0,
            NumberOfDuelsDraw = 0
        }
    };
    */

    [HttpGet("GetAllPlayerStatistics")]
    public IActionResult GetAllPlayerStatistics() {
        return Ok(playerStatistics);
    }

    /*
    [HttpPost("UpdatePlayerStatistics")]
    public IActionResult UpdatePlayerStatistics([FromBody] DuelOutcome duelOutcome)
    {
        if (duelOutcome == null) {
            return BadRequest("Invalid duel outcome data.");
        }
        
        int winnerId = duelOutcome.WinnerId;
        int loserId = duelOutcome.LoserId;

        if (!playerStatistics.ContainsKey(winnerId))
        {
            playerStatistics[winnerId] = new PlayerStatistics();
        }

        if (!playerStatistics.ContainsKey(loserId))
        {
            playerStatistics[loserId] = new PlayerStatistics();
        }

        // Update statistics for the winner
        playerStatistics[winnerId].NumberOfDuelsWon++;
        playerStatistics[winnerId].NumberOfDuelsPlayed++;

        // Update statistics for the loser
        playerStatistics[loserId].NumberOfDuelsLost++;
        playerStatistics[loserId].NumberOfDuelsPlayed++;
        
        if (duelOutcome.IsDraw)
        {
            playerStatistics[winnerId].NumberOfDuelsDraw++;
            playerStatistics[loserId].NumberOfDuelsDraw++;
        }
        return Ok("Player statistics updated.");
    }
    */ 
    private PlayerStatistics FindPlayerStatisticsById(int playerId) {
        return playerStatistics.FirstOrDefault(p => p.PlayerId == playerId);
    }
    

    [HttpPost("UpdatePlayerStatistics")]
    public IActionResult UpdatePlayerStatistics([FromBody] Duel duelOutcome) {
        Console.WriteLine("hier2");
      //  if (!playerStatistics.Any())
      //  {
            PlayerStatistics ll = new PlayerStatistics
            {
                PlayerId = duelOutcome.Player1.Id,
                PlayerName = duelOutcome.Player1.Name,
                CurrentEloRating = duelOutcome.Player1.EloRating,
                LastDuelPlayedAt = DateTime.Parse("2023-11-26T19:46:14.262Z"),

                NumberOfDuelsPlayed = 0,
                NumberOfDuelsWon = 0,
                NumberOfDuelsLost = 0,
                NumberOfDuelsDraw = 0
            };
            playerStatistics.Add(ll);
       // }
            /*
        if (duelOutcome == null) {
            return BadRequest("Invalid duel outcome data.");
        }

        int winnerId = duelOutcome.WinnerId;
        int loserId = duelOutcome.LoserId;

        PlayerStatistics winnerStats = FindPlayerStatisticsById(winnerId);
        PlayerStatistics loserStats = FindPlayerStatisticsById(loserId);

        if (winnerStats == null) {
            return BadRequest($"Player with ID {winnerId} not found.");
        }

        if (loserStats == null) {
            return BadRequest($"Player with ID {loserId} not found.");
        }

        // Aktualisiere die Statistiken für den Gewinner
        winnerStats.NumberOfDuelsWon++;
        winnerStats.NumberOfDuelsPlayed++;

        // Aktualisiere die Statistiken für den Verlierer
        loserStats.NumberOfDuelsLost++;
        loserStats.NumberOfDuelsPlayed++;

        if (duelOutcome.IsDraw) {
            winnerStats.NumberOfDuelsDraw++;
            loserStats.NumberOfDuelsDraw++;
        }
        */
        return Ok("Player statistics updated.");
    }
}

public class Duel
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    
    public DateTime ScheduledTime { get; set; }
    
    public DuelResult DuelResult { get; set; }
}
public enum DuelResult
{
    Won,
    Lost,
    Draw
}