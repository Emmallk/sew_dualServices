using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchmakingService;
using RegistrationService;
using System.Text.Json;

namespace DuelSimulation.Controllers;

[Route("DuelSimulation")]
public class DuelSimulationController : Controller
{
    private static HttpClient _httpClient = new HttpClient();
    List<Duel> upcomingDuels = new List<Duel>();

    [HttpGet("GetMatchedPlayers")]
    public async Task<IActionResult> GetMatchedPlayers()
    {
        try
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync("http://localhost:5099/Matchmaking/GetAllPlayers");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                upcomingDuels = JsonSerializer.Deserialize<List<Duel>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Ok(upcomingDuels);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Fehler beim Abrufen von Spielern von RegistrationService");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
        }
    }

    [HttpPost("SimulateDuels")]
    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var duel in upcomingDuels)
            {
                int outcome = SimulateDuelOutcome();
                int eloDelta = CalculateEloDelta(duel.Player1.EloRating, duel.Player2.EloRating);

                if (outcome == 1) // Player 1 wins
                {
                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating + eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating - eloDelta);
                    UpdateStatistics(duel); //duel.Player1, duel.Player2, outcome
                }
                else if (outcome == 2) // Player 2 wins
                {
                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating - eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating + eloDelta);
                    UpdateStatistics(duel); //duel.Player1, duel.Player2, outcome
                }
                else if (outcome == 0) // No change in Elo ratings for a draw
                {
                     UpdateStatistics(duel);
                }
            }

            await Task.Delay(10_000);
        }
    }
    
    private int SimulateDuelOutcome()
    {
        Random random = new Random();
        int outcome = random.Next(0, 3);
        return outcome;
    }

    private double ExpectationToWin(int playerOneRating, int playerTwoRating)
    {
        return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
    }

    private int CalculateEloDelta(int playerOneRating, int playerTwoRating)
    {
        int eloK = 32;
        return (int)(eloK * (1 - ExpectationToWin(playerOneRating, playerTwoRating)));
    }

    private async Task UpdateEloRating(int playerId, int neweloDelta)
    {
        await _httpClient.PutAsync($"http://localhost:5098/Registration/UpdateEloRating/{playerId}/{neweloDelta}",
            null);
    }
    private async Task UpdateStatistics(Duel duel)
    {
        await _httpClient.PutAsync($"http://localhost:5100/Statistics/UpdateStatistics",
            new StringContent(JsonSerializer.Serialize(duel)));
    }


    /*
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
                await Task.Delay(10000);
                continue;
            }
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
                int outcome = SimulateDuelOutcome();
                int eloDelta = CalculateEloDelta(duel.Player1.EloRating, duel.Player2.EloRating, outcome);
                if (outcome == 1) // Player 1 wins
                {
                    await registrationServiceClient.UpdateElo(duel.Player1.Id, eloDelta);
                    await registrationServiceClient.UpdateElo(duel.Player2.Id, -eloDelta);
                    await statisticsServiceClient.UpdateGeneralStatistics(duel.Player1, duel.Player2, outcome);
                }
                else if (outcome == 2) // Player 2 wins
                {
                    await registrationServiceClient.UpdateElo(duel.Player1.Id, -eloDelta);
                    await registrationServiceClient.UpdateElo(duel.Player2.Id, eloDelta);
                    await statisticsServiceClient.UpdateGeneralStatistics(duel.Player2, duel.Player1, outcome);
                }
                else if (outcome == 0) // No change in Elo ratings for a draw
                {
                    await statisticsServiceClient.UpdateGeneralStatistics(duel.Player1, duel.Player2, outcome);
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
        int outcome = random.Next(0, 3); // Generates a random number between 1 and 3
        if (outcome == 1 || outcome == 2) {
            return outcome; // 1 or 2 means either Player 1 or Player 2 wins
        }
        else {
            return 0;
        }
    }

    private int CalculateEloDelta(int playerOneRating, int playerTwoRating, int outcome)
    {
        //int eloK = 32;
        //double expectationToWin = 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
        //int eloDelta = (int)(eloK * (playerOneWins ? (1 - expectationToWin) : (-expectationToWin)));
        //return eloDelta;

        int eloK = 32;
        return (int)(eloK * (1 - ExpectationToWin(playerOneRating, playerTwoRating)));
    }
    private double ExpectationToWin(int playerOneRating, int playerTwoRating)
    {
        return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
    }
    */
}