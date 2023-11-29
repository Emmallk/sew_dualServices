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

//[Route("api/[controller]")]
[Route("Matchmaking")]
[ApiController]
public class MatchmakingServiceController : Controller
{
    private List<Player> players;
    private List<PlayerStatistics> playerStatistics;
    private static HttpClient _httpClient = new HttpClient();
    
    [HttpGet("GetPlayers")]
    public async Task<IActionResult> GetPlayers() {
        try {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5099/Registration/GetAllPlayers"); // wenn man die daten im Matchmaking ausführt
            //HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5254/Registration/GetAllPlayers"); // wenn man die daten im Registration Tap ausführt
            if (response.IsSuccessStatusCode) {
                string content = await response.Content.ReadAsStringAsync();
                players = JsonSerializer.Deserialize<List<Player>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Ok(players);
            } 
            else {
                return StatusCode((int)response.StatusCode, "Fehler beim Abrufen von Spielern von RegistrationService");
            }
        }
        catch (Exception ex) {
            return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
        }
    }
    [HttpGet("GetStatistic")]
    public async Task<IActionResult> GetStatistic() {
        try {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5150/Registration/GetAllPlayers");
            if (response.IsSuccessStatusCode) {
                string content = await response.Content.ReadAsStringAsync();
                playerStatistics = JsonSerializer.Deserialize<List<PlayerStatistics>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Ok(players);
            } 
            else {
                return StatusCode((int)response.StatusCode, "Fehler beim Abrufen von Spielern von RegistrationService");
            }
        }
        catch (Exception ex) {
            return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
        }
    }
    /*
    private readonly RegistrationServiceClient registrationServiceClient;
    private readonly StatisticsServiceClient statisticsServiceClient; 
    public MatchmakingServiceController(RegistrationServiceClient registrationServiceClient, StatisticsServiceClient statisticsServiceClient, PlayerService playerService)
    {
        this.registrationServiceClient = registrationServiceClient;
        this.statisticsServiceClient = statisticsServiceClient;
        _playerService = playerService;
    } 
    */
    /*[HttpGet]
    public async Task<IActionResult> GetMatchmaking()
    {
        var registeredPlayers = await registrationServiceClient.GetAllPlayers();
        var playerStatistics = await statisticsServiceClient.GetPlayerStatistics();

        var orderedPlayers = registeredPlayers
            .OrderBy(p => playerStatistics.ContainsKey(p.Id) ? playerStatistics[p.Id].LastDuelPlayedAt ?? DateTime.MinValue : DateTime.MinValue)
            .ToList();

        var upcomingDuels = GenerateUpcomingDuels(orderedPlayers);
        return Ok(upcomingDuels);
    }*/
    
    [HttpGet("Matchmaking")]
    public async Task<IActionResult> MatchmakePlayers()
    {
        if (players.Count < 2)
            return BadRequest("Insufficient players for matchmaking.");
        var eligiblePlayers = players.Where(p => playerStatistics.Any(ps => ps.PlayerId == p.Id) && p.EloRating > 1000);

        if (!eligiblePlayers.Any())
            return BadRequest("No eligible players for matchmaking.");
        
        eligiblePlayers = eligiblePlayers.OrderBy(p => {
            var playerStat = playerStatistics.FirstOrDefault(ps => ps.PlayerId == p.Id);
            return playerStat?.LastDuelPlayedAt ?? DateTime.MinValue;
        });

        List<Duel> upcomingDuels = new List<Duel>();

        foreach (var player in eligiblePlayers)
        {
            var opponent = FindBestOpponent(player, eligiblePlayers, upcomingDuels);

            if (opponent != null)
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
        }
        return Ok(upcomingDuels);
    }

    private Player FindBestOpponent(Player player, IEnumerable<Player> eligiblePlayers, IEnumerable<Duel> upcomingDuels)
    {
        // Filtern Sie bereits ausgewählte Gegner aus
        var availableOpponents = eligiblePlayers.Where(p => p.Id != player.Id && !IsPlayerSelected(p, upcomingDuels));
        // Sortieren Sie Gegner nach Elo-Differenz
        availableOpponents = availableOpponents.OrderBy(p => Math.Abs(p.EloRating - player.EloRating));
        return availableOpponents.FirstOrDefault();
    }

    private bool IsPlayerSelected(Player player, IEnumerable<Duel> upcomingDuels)
    {
        // Überprüfen Sie, ob der Spieler bereits in einem bevorstehenden Duell ausgewählt wurde
        return upcomingDuels.Any(duel => duel.Player1.Id == player.Id || duel.Player2.Id == player.Id);
    }

    
    /*
    public async Task<IActionResult> MatchmakePlayers() {
        if (players.Count < 2) {
            return BadRequest("Insufficient players for matchmaking.");
        }
        var eligiblePlayers = players.Where(p => playerStatistics.Any(ps => ps.PlayerId == p.Id) && p.EloRating > 1000);

        if (!eligiblePlayers.Any()) {
            return BadRequest("No eligible players for matchmaking.");
        }
        List<Duel> upcomingDuels = new List<Duel>();
        foreach (var player in eligiblePlayers) {
            var opponent = eligiblePlayers.FirstOrDefault(p => p.Id != player.Id);

            if (opponent != null) {
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
            }
        }
        return Ok(upcomingDuels);
    }
    */

    /*private List<Duel> GenerateUpcomingDuels(List<Player> orderedPlayers)
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
    }*/
}
