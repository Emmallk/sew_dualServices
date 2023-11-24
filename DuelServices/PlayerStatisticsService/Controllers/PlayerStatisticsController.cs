using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using PlayerStatisticsService.Class;
using RegistrationService.Class;

namespace PlayerStatisticsService.Controllers;

public class PlayerStatisticsController : ControllerBase
{
    private readonly List<Player> _players; // Annahme: Hier werden Ihre Spielerdaten gespeichert

    // Konstruktor, um die Spielerliste zu initialisieren
    public PlayerStatisticsController(List<Player> players)
    {
        _players = players;
    }

    [HttpGet]
    [Route("api/playerstatistics/{playerId}")]
    public IActionResult GetPlayerStatistics(int playerId)
    {
        var playerStatistics = GetPlayerStatisticsFromList(playerId);

        if (playerStatistics != null)
        {
            return Ok(playerStatistics);
        }
        else
        {
            return NotFound($"Spieler mit der ID {playerId} wurde nicht gefunden.");
        }
    }

    private PlayerStatistics GetPlayerStatisticsFromList(int playerId)
    {
        // Implementieren Sie hier die Logik zum Abrufen der Spielerstatistiken aus der Liste
        // Beispiel:
        var player = _players.FirstOrDefault(p => p.Id == playerId);

        if (player != null)
        {
            /*
            // Hier sollten Sie die Logik implementieren, um die Spielerstatistiken aus den Duellen oder anderweitig zu berechnen
            // Beispiel:
            // var wins = ...;
            // var losses = ...;

            // Annahme: Hier wird eine neue Instanz von PlayerStatistics erstellt
            var playerStatistics = new PlayerStatistics
            {
                PlayerId = playerId,
                Wins = wins,
                Losses = losses
            };

            return playerStatistics;
            */
        }
        return null;
    }
    
    
    
    
    
    private static List<PlayerStatistics> playerStatisticsList = new List<PlayerStatistics>();

    [HttpGet]
    public ActionResult<IEnumerable<PlayerStatistics>> Get()
    {
        return Ok(playerStatisticsList);
    }

    [HttpPost]
    public ActionResult UpdateStatistics(PlayerStatistics updatedStatistics)
    {
        // Find the player in the list and update their statistics
        PlayerStatistics playerToUpdate = playerStatisticsList.Find(p => p.PlayerId == updatedStatistics.PlayerId);

        if (playerToUpdate != null)
        {
            // Update statistics
            playerToUpdate.NumberOfDuelsWon = updatedStatistics.NumberOfDuelsWon;
            playerToUpdate.NumberOfDuelsLost = updatedStatistics.NumberOfDuelsLost;
            playerToUpdate.NumberOfDuelsDraw = updatedStatistics.NumberOfDuelsDraw;
            playerToUpdate.NumberOfDuelsPlayed = updatedStatistics.NumberOfDuelsPlayed;
            playerToUpdate.AverageDuelDuration = updatedStatistics.AverageDuelDuration;
            playerToUpdate.LastDuelPlayedAt = updatedStatistics.LastDuelPlayedAt;

            return Ok();
        }
        else
        {
            return NotFound($"Player with ID {updatedStatistics.PlayerId} not found.");
        }
    }

}

