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
    private static List<PlayerStatistics> playerStatistics = new List<PlayerStatistics>();


    //Endpunkt GET /Statistics
    [HttpGet("GetAllPlayerStatistics")]
    public IActionResult GetAllPlayerStatistics() {
        return Ok(playerStatistics);
    }

    //Endpunkt POST /Statistics
    [HttpPost("UpdatePlayerStatistics")]
    public IActionResult UpdatePlayerStatistics([FromBody] Duel duelOutcome) {
        //Überprüfen ob Spieler schon in der Liste playerStatistics vorhanden ist
        if (playerStatistics.Any(p => p.PlayerId == duelOutcome.Player1.Id)) {
            //Auslesen des Existierenden Players
            PlayerStatistics existingPlayer1 =
                playerStatistics.FirstOrDefault(p => p.PlayerId == duelOutcome.Player1.Id);

            //Überschreiben der Werte
            if (existingPlayer1 != null) {
                // Updaten die Spielerstatistiken
                existingPlayer1.NumberOfDuelsPlayed++;
                if (duelOutcome.DuelResult == 1) {
                    existingPlayer1.NumberOfDuelsWon++;
                }
                else if (duelOutcome.DuelResult == 2) {
                    existingPlayer1.NumberOfDuelsLost++;
                }
                else {
                    existingPlayer1.NumberOfDuelsDraw++;
                }

                //Updaten von EloRating & LastDuelPlayedAt
                existingPlayer1.CurrentEloRating = duelOutcome.Player1.EloRating;
                existingPlayer1.LastDuelPlayedAt = DateTime.UtcNow;
            }
        }
        else {
            //Wenn der Player noch nicht vorhanden ist -> Werte reinschreiben
            PlayerStatistics player1 = new PlayerStatistics {
                PlayerId = duelOutcome.Player1.Id,
                PlayerName = duelOutcome.Player1.Name,
                CurrentEloRating = duelOutcome.Player1.EloRating,
                LastDuelPlayedAt = DateTime.Parse("2023-11-26T19:46:14.262Z"),

                NumberOfDuelsPlayed = 1,
                NumberOfDuelsWon = duelOutcome.DuelResult == 1 ? 1 : 0,
                NumberOfDuelsLost = duelOutcome.DuelResult == 2 ? 1 : 0,
                NumberOfDuelsDraw = duelOutcome.DuelResult == 0 ? 1 : 0
            };
            //Spieler 1 wird zur Liste hinzugefügt
            playerStatistics.Add(player1);
        }

        if (playerStatistics.Any(p => p.PlayerId == duelOutcome.Player2.Id)) {
            PlayerStatistics existingPlayer2 =
                playerStatistics.FirstOrDefault(p => p.PlayerId == duelOutcome.Player2.Id);

            if (existingPlayer2 != null) {
                existingPlayer2.NumberOfDuelsPlayed++;
                if (duelOutcome.DuelResult == 2) {
                    existingPlayer2.NumberOfDuelsWon++;
                }
                else if (duelOutcome.DuelResult == 1) {
                    existingPlayer2.NumberOfDuelsLost++;
                }
                else {
                    existingPlayer2.NumberOfDuelsDraw++;
                }

                existingPlayer2.CurrentEloRating = duelOutcome.Player2.EloRating;
                existingPlayer2.LastDuelPlayedAt = DateTime.UtcNow;
            }
        }
        else {
            PlayerStatistics player2 = new PlayerStatistics {
                PlayerId = duelOutcome.Player2.Id,
                PlayerName = duelOutcome.Player2.Name,
                CurrentEloRating = duelOutcome.Player2.EloRating,
                LastDuelPlayedAt = DateTime.Parse("2023-11-26T19:46:14.262Z"),

                NumberOfDuelsPlayed = 1,
                NumberOfDuelsWon =
                    duelOutcome.DuelResult == 2
                        ? 1
                        : 0, // Wenn duelOutcome.DuelResult gleich 1 ist, dann 1 hinzufügen, ansonsten 0 behalten
                NumberOfDuelsLost = duelOutcome.DuelResult == 1 ? 1 : 0,
                NumberOfDuelsDraw = duelOutcome.DuelResult == 0 ? 1 : 0
            };
            playerStatistics.Add(player2);
        }

        return Ok("Player statistics updated.");
    }
}