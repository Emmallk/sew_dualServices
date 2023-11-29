using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using RegistrationService;

namespace PlayerStatisticsService.Controllers;
[ApiController]
[Route("Statistics")]
public class PlayerStatisticsController : ControllerBase
    {
        private readonly Dictionary<int, PlayerStatistics> playerStats = new Dictionary<int, PlayerStatistics>(); // Storing player statistics

        [HttpGet]
        public IActionResult GetAllPlayerStatistics()
        {
            return Ok(playerStats.Values);
        }

        [HttpPost]
        public IActionResult UpdatePlayerStatistics([FromBody] DuelOutcome duelOutcome)
        {
            if (duelOutcome == null)
            {
                return BadRequest("Invalid duel outcome data.");
            }

            // Assuming duelOutcome contains the IDs of the players involved and the outcome (win, loss, draw)
            int winnerId = duelOutcome.WinnerId;
            int loserId = duelOutcome.LoserId;

            if (!playerStats.ContainsKey(winnerId))
            {
                playerStats[winnerId] = new PlayerStatistics();
            }

            if (!playerStats.ContainsKey(loserId))
            {
                playerStats[loserId] = new PlayerStatistics();
            }

            // Update statistics for the winner
            playerStats[winnerId].NumberOfDuelsWon++;
            playerStats[winnerId].NumberOfDuelsPlayed++;

            // Update statistics for the loser
            playerStats[loserId].NumberOfDuelsLost++;
            playerStats[loserId].NumberOfDuelsPlayed++;

            // For a draw scenario (if applicable)
            if (duelOutcome.IsDraw)
            {
                playerStats[winnerId].NumberOfDuelsDraw++;
                playerStats[loserId].NumberOfDuelsDraw++;
            }

            // Update other statistics as needed (e.g., average duel duration, last duel played, etc.)

            return Ok("Player statistics updated.");
        }
    }