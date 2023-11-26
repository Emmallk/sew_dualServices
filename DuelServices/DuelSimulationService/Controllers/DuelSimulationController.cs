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
                SimulateDuel(duel);
            }
            await Task.Delay(10_000);
        }
    }

    private void SimulateDuel(Duel duel)
    {
        var random = new Random();  // Simulation random gewinner
        var result = random.Next(0, 3); // 0: Draw, 1: Player1 wins, 2: Player2 wins

        switch (result)
        {
            case 0: // Draw, no Elo changes
                break;
            case 1: // Player1 wins
                UpdateElo(duel.Player1, duel.Player2);
                break;
            case 2: // Player2 wins
                UpdateElo(duel.Player2, duel.Player1);
                break;
        }

        // Update general statistics (you can customize this based on your actual needs)
        UpdateGeneralStatistics(duel, result);
    }

    private void UpdateElo(Player winner, Player loser)
    {
        var kFactor = 32;

        var expectedWinProbability = 1.0 / (1.0 + Math.Pow(10, (loser.EloRating - winner.EloRating) / 400.0));
        var eloChange = (int)(kFactor * (1 - expectedWinProbability));

        winner.EloRating += eloChange;
        loser.EloRating -= eloChange;

        // Update Elo values in the Registration Service
        registrationServiceClient.UpdateElo(winner.Id, winner.EloRating);
        registrationServiceClient.UpdateElo(loser.Id, loser.EloRating);
    }

    /*
    private double ExpectationToWin(int playerOneRating, int playerTwoRating)
    {
        return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
    }

    private int CalculateEloDelta(int playerOneRating, int playerTwoRating)
    {
        int eloK = 32;
        return (int)(eloK * (1 - ExpectationToWin(playerOneRating, playerTwoRating)));
    }
    */
    private void UpdateGeneralStatistics(Duel duel, int result)
    {
        statisticsServiceClient.UpdateGeneralStatistics(duel, result);
    }
}