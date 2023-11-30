using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayerStatisticsService;
using RegistrationService;
using System.Text.Json;

namespace MatchmakingService.Controllers;

[Route("Matchmaking")]
[ApiController]
public class MatchmakingServiceController : Controller {
    //private List<Player> players = new List<Player>();
    //private List<PlayerStatistics> playerStatistics = new List<PlayerStatistics>();
    private static HttpClient _httpClient = new HttpClient();

    //HTTP Clients von Registration und Statistics Servies (siehe Files)
    private RegistrationServiceClient _registrationService = new RegistrationServiceClient(_httpClient);
    private StatisticsServiceClient _statisticsServiceClient = new StatisticsServiceClient(_httpClient);
    

    [HttpGet("Matchmaking")]
    public async Task<IActionResult> MatchmakePlayers()
    {
        List<Player> players =  await _registrationService.GetPlayersFromRemoteService();
        List<PlayerStatistics> playerStatistics = await _statisticsServiceClient.GetPlayerStatisticsFromRemoteService();
        
        List<Player> sortedPlayers = SortPlayers(players, playerStatistics);
        List<Duel> upcomingDuels = GenerateUpcomingDuels(sortedPlayers);
        
        return Ok(upcomingDuels);
    }

    private List<Player> SortPlayers(List<Player> players, List<PlayerStatistics> playerStatistics)
    {
        // Sortieren nach EloRating und GetLastDuelTime
        List<Player> sortedPlayers = players.OrderByDescending(p => p.EloRating)
            .ThenBy(p => GetLastDuelTime(playerStatistics, p.Id))
            .ToList();
        return sortedPlayers;
    }

    private DateTime? GetLastDuelTime(List<PlayerStatistics> playerStatistics, int playerId)
    {
        return playerStatistics.FirstOrDefault(ps => ps.PlayerId == playerId)?.LastDuelPlayedAt;
    }

    private List<Duel> GenerateUpcomingDuels(List<Player> sortedPlayers)
    {
        List<Duel> upcomingDuels = new List<Duel>();
        // ähnlichsten EloRating und je nach LetztenGame Spieler zusammen paaren
        for (int i = 0; i < sortedPlayers.Count - 1; i += 2)
        {
            Duel duel = new Duel
            {
                Player1 = sortedPlayers[i],
                Player2 = sortedPlayers[i + 1],

            };
            upcomingDuels.Add(duel);
        }
        return upcomingDuels;
    }
}