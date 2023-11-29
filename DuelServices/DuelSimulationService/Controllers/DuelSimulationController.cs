using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchmakingService;
using RegistrationService;

namespace DuelSimulation.Controllers;

[Route("DuelSimulation")]
public class DuelSimulationController : Controller
{
    private readonly MatchmakingServiceClient matchmakingServiceClient;
    private readonly RegistrationServiceClient registrationServiceClient;
    private readonly StatisticsServiceClient statisticsServiceClient;

    public DuelSimulationController(
        MatchmakingServiceClient matchmakingServiceClient,
        RegistrationServiceClient registrationServiceClient,
        StatisticsServiceClient statisticsServiceClient)
    {
        this.matchmakingServiceClient = matchmakingServiceClient;
        this.registrationServiceClient = registrationServiceClient;
        this.statisticsServiceClient = statisticsServiceClient;
    }

    [HttpGet("SimulateDuels")]
    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Get upcoming duels from the matchmaking service
            var upcomingDuels = await matchmakingServiceClient.GetMatchmaking();
            if (upcomingDuels.Count() == 0)
            {
                // No upcoming duels, wait and check again
                await Task.Delay(10000); 
                continue;
            }
            // Simulate and update Elo for each duel
            foreach (var duel in upcomingDuels)
            {
                SimulateDuels();
            }
            await Task.Delay(10_000);
        }
    }

    [HttpPost("SimulateDuels")]
    public async Task<IActionResult> SimulateDuels()
    {
        try
        {
            List<Duel> upcomingDuels = await matchmakingServiceClient.GetMatchmaking();

            if (upcomingDuels.Count == 0)
            {
                return BadRequest("No upcoming duels found.");
            }

            foreach (var duel in upcomingDuels)
            {
                Player player1 = duel.Player1;
                Player player2 = duel.Player2;

                // Simulate duel outcome (assuming 33% chance for each outcome - win, lose, draw)
                int outcome = SimulateDuelOutcome();

                int eloDelta = CalculateEloDelta(player1.EloRating, player2.EloRating, outcome);

                if (outcome == 1) // Player 1 wins
                {
                    await registrationServiceClient.UpdateEloRating(player1.Id, eloDelta);
                    await registrationServiceClient.UpdateEloRating(player2.Id, -eloDelta);
                    await UpdatePlayerStatistics(player1.Id, player2.Id, true);
                }
                else if (outcome == -1) // Player 2 wins
                {
                    await registrationServiceClient.UpdateEloRating(player1.Id, -eloDelta);
                    await registrationServiceClient.UpdateEloRating(player2.Id, eloDelta);
                    await UpdatePlayerStatistics(player2.Id, player1.Id, true);
                }
                else // Draw
                {
                    // No change in Elo ratings for a draw
                    await UpdatePlayerStatistics(player1.Id, player2.Id, false);
                }
            }

            return Ok("Duels simulated and player data updated.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error simulating duels: {ex.Message}");
        }
    }

    // SimulateDuelOutcome method to randomly determine the outcome of a duel
    private int SimulateDuelOutcome()
    {
        Random random = new Random();
        int outcome = random.Next(1, 4); // Generates a random number between 1 and 3

        if (outcome == 1 || outcome == 2)
        {
            return outcome; // 1 or 2 means either Player 1 or Player 2 wins
        }
        else
        {
            return 0; // 0 means a draw
        }
    }

    private int CalculateEloDelta(int playerOneRating, int playerTwoRating, bool playerOneWins)
    {
        int eloK = 32;
        double expectationToWin = 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
        int eloDelta = (int)(eloK * (playerOneWins ? (1 - expectationToWin) : (-expectationToWin)));
        return eloDelta;
    }

    private async Task UpdatePlayerStatistics(int playerId1, int playerId2, bool player1Wins)
    {
        if (player1Wins)
        {
            await statisticsServiceClient.UpdateStatistics(playerId1, playerId2, true);
        }
        else
        {
            await statisticsServiceClient.UpdateStatistics(playerId2, playerId1, false);
        }
    }
}