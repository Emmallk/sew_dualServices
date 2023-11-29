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
        private readonly Dictionary<int, PlayerStatistics> playerStatistics = new Dictionary<int, PlayerStatistics>(); // Storing player statistics

        [HttpGet]
        public IActionResult GetAllPlayerStatistics() {
            return Ok(playerStatistics.Values);
        } 

        [HttpPost]
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
    }