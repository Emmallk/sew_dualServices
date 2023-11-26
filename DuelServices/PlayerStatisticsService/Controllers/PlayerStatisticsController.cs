using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using RegistrationService;

namespace PlayerStatisticsService.Controllers;
[Route("Statistics")]
    public class PlayerStatisticsController : Controller
    {
        private static List<PlayerStatistics> playerStatisticsList = new List<PlayerStatistics>();

        [HttpGet]
        public IActionResult GetPlayerStatistics()
        {
            return Ok(playerStatisticsList);
        }

        [HttpPost]
        public IActionResult UpdatePlayerStatistics([FromBody] DuelOutcome duelOutcome)
        {
            var player1Statistics = GetOrCreatePlayerStatistics(duelOutcome.Player1Id, duelOutcome.Player1Name);
            var player2Statistics = GetOrCreatePlayerStatistics(duelOutcome.Player2Id, duelOutcome.Player2Name);

            UpdateStatistics(player1Statistics, duelOutcome.Result);
            UpdateStatistics(player2Statistics, duelOutcome.Result);

            return Ok();
        }

        private PlayerStatistics GetOrCreatePlayerStatistics(int playerId, string playerName)
        {
            var playerStatistics = playerStatisticsList.FirstOrDefault(p => p.PlayerId == playerId);
            if (playerStatistics == null)
            {
                playerStatistics = new PlayerStatistics
                {
                    PlayerId = playerId,
                    PlayerName = playerName
                };
                playerStatisticsList.Add(playerStatistics);
            }
            return playerStatistics;
        }

        private void UpdateStatistics(PlayerStatistics playerStatistics, DuelResult result)
        {
            playerStatistics.NumberOfDuelsPlayed++;
            switch (result)
            {
                case DuelResult.Won:
                    playerStatistics.NumberOfDuelsWon++;
                    break;
                case DuelResult.Lost:
                    playerStatistics.NumberOfDuelsLost++;
                    break;
                case DuelResult.Draw:
                    playerStatistics.NumberOfDuelsDraw++;
                    break;
            }

            // Update other statistics as needed
            // ...

            playerStatistics.LastDuelPlayedAt = System.DateTime.UtcNow;
        }
    }