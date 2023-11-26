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
        return upcomingDuels;
    }
}