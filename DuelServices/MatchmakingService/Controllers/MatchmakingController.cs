using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PlayerStatisticsService.Interfaces;
using RegistrationService;

namespace MatchmakingService.Controllers;


[Route("Matchmaking")]
public class MatchmakingServiceController : Controller
{
    private readonly RegistrationServiceClient registrationServiceClient;
    private readonly StatisticsServiceClient statisticsServiceClient;

    public MatchmakingServiceController(RegistrationServiceClient registrationServiceClient, StatisticsServiceClient statisticsServiceClient)
    {
        this.registrationServiceClient = registrationServiceClient;
        this.statisticsServiceClient = statisticsServiceClient;
    }

    // GET /Matchmaking
    [HttpGet]
    public async Task<IActionResult> GetMatchmaking()
    {
        var registeredPlayers = await registrationServiceClient.GetAllPlayers();
        var playerStatistics = await statisticsServiceClient.GetPlayerStatistics();

        var orderedPlayers = registeredPlayers
            .OrderBy(p => playerStatistics.ContainsKey(p.Id) ? playerStatistics[p.Id].LastDuelPlayedAt ?? DateTime.MinValue : DateTime.MinValue)
            .ToList();

        var upcomingDuels = GenerateUpcomingDuels(orderedPlayers);
        return Ok(upcomingDuels);
    }


    private List<Duel> GenerateUpcomingDuels(List<Player> orderedPlayers)
    {
        var upcomingDuels = new List<Duel>();
        for (int i = 0; i < orderedPlayers.Count - 1; i++)
        {
            for (int j = i + 1; j < orderedPlayers.Count; j++)
            {
                if (Math.Abs(orderedPlayers[i].EloRating - orderedPlayers[j].EloRating) <= 100)
                {
                    var duel = new Duel
                    {
                        Player1 = orderedPlayers[i],
                        Player2 = orderedPlayers[j],
                        ScheduledTime = DateTime.Now.AddHours(1) 
                    };
                    upcomingDuels.Add(duel);
                }
            }
        }
<<<<<<< HEAD
=======

        // Fügen Sie einen Rückgabewert hinzu (z.B. NotFound), wenn die Anfrage nicht erfolgreich ist
        return NotFound();
    }


    [HttpGet]
    public ActionResult<string> Get([FromQuery] string parameter1, [FromQuery] string parameter2)
    {
        string result = $"Daten erhalten: Parameter1 = {parameter1}, Parameter2 = {parameter2}";

        return result;
    }

    private List<Duel> GetUpcomingDuels(List<Player> players)
    {
        List<Duel> upcomingDuels = new List<Duel>();

        // Logic to match players based on Elo values
        // You need to implement your own logic here based on your requirements

        // Example: Matching players with similar Elo values
        foreach (var player in players)
        {
            var opponent = players.Where(p => p.Id != player.Id)
                .OrderBy(p => Math.Abs(p.EloRating - player.EloRating))
                .FirstOrDefault();

            if (opponent != null)
            {
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
            }
        }

>>>>>>> bf4355916aae301bcf91af9278502d4f1655710f
        return upcomingDuels;
    }
}