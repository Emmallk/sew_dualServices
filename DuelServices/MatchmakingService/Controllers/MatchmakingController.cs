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
    private List<Player> players = new List<Player>();
    private List<PlayerStatistics> playerStatistics = new List<PlayerStatistics>();
    private static HttpClient _httpClient = new HttpClient();

    private RegistrationServiceClient _registrationService = new RegistrationServiceClient(_httpClient);

    private StatisticsServiceClient _statisticsServiceClient = new StatisticsServiceClient(_httpClient);

    /*
    // [HttpGet("GetPlayers")] 
    public async Task<List<Player>> GetPlayers() {
     //   try {
            HttpResponseMessage response =
                await _httpClient.GetAsync(
                    "http://localhost:5099/Registration/GetAllPlayers"); // wenn man die daten im Matchmaking ausführt
            //HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5254/Registration/GetAllPlayers"); // wenn man die daten im Registration Tap ausführt
            if (response.IsSuccessStatusCode) {
                string content = await response.Content.ReadAsStringAsync();
                players = JsonSerializer.Deserialize<List<Player>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return players;
            }

            return null;
            /*
            else {
                // return StatusCode((int)response.StatusCode, "Fehler beim Abrufen von Spielern von RegistrationService");
                return players;
            }
        }
        catch (Exception ex) {
            // return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
            return players;
        }
    }
    */
    /*
    [HttpGet("GetStatistic")]
    public async Task<List<PlayerStatistics>> GetStatistic() {
        //try {
            HttpResponseMessage response =
                await _httpClient.GetAsync("http://localhost:5099/Statistics/GetAllPlayerStatistics");
            if (response.IsSuccessStatusCode) {
                string content = await response.Content.ReadAsStringAsync();
                playerStatistics = JsonSerializer.Deserialize<List<PlayerStatistics>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return playerStatistics;
            }
            return null;
            /*
            else {
                //return StatusCode((int)response.StatusCode, "Fehler beim Abrufen von Spielern von RegistrationService");
                return playerStatistics;
            }
        }
        catch (Exception ex) {
            //return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
            return playerStatistics;
        }
        
    }

*/

    /*
    [HttpGet("GetMatchmaking")]
    public async Task<IActionResult> MatchmakePlayers() {
        players = _registrationService.GetPlayersFromRemoteService();
        playerStatistics = _statisticsServiceClient.GetPlayerStatisticsFromRemoteService();
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

        foreach (var player in eligiblePlayers) {
            var opponent = FindBestOpponent(player, eligiblePlayers, upcomingDuels);

            if (opponent != null)
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
        }
        return Ok(upcomingDuels);
    }

    private Player FindBestOpponent(Player player, IEnumerable<Player> eligiblePlayers,
        IEnumerable<Duel> upcomingDuels) {
        // Filtern Sie bereits ausgewählte Gegner aus
        var availableOpponents = eligiblePlayers.Where(p => p.Id != player.Id && !IsPlayerSelected(p, upcomingDuels));
        // Sortieren Sie Gegner nach Elo-Differenz
        availableOpponents = availableOpponents.OrderBy(p => Math.Abs(p.EloRating - player.EloRating));
        return availableOpponents.FirstOrDefault();
    }

    private bool IsPlayerSelected(Player player, IEnumerable<Duel> upcomingDuels) {
        // Überprüfen Sie, ob der Spieler bereits in einem bevorstehenden Duell ausgewählt wurde
        return upcomingDuels.Any(duel => duel.Player1.Id == player.Id || duel.Player2.Id == player.Id);
    }
    */
    
    // TODO: aswahlkriterien hinterfragen
    
    [HttpGet("Matchmaking")]
    public async Task<IActionResult> MatchmakePlayers()
    {
        List<Player> players = _registrationService.GetPlayersFromRemoteService();
        List<PlayerStatistics> playerStatistics = _statisticsServiceClient.GetPlayerStatisticsFromRemoteService();
        
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
                ScheduledTime = DateTime.Now
            };
            upcomingDuels.Add(duel);
        }
        return upcomingDuels;
    }
    
    /*
    [HttpGet("Matchmaking")]
    public async Task<IActionResult> MatchmakePlayers() {
        players = _registrationService.GetPlayersFromRemoteService();
        playerStatistics = _statisticsServiceClient.GetPlayerStatisticsFromRemoteService();
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
        List<Player> selectedPlayers = new List<Player>(); // Liste für bereits ausgewählte Spieler

        foreach (var player in eligiblePlayers) {
            var opponent = FindBestOpponent(player, eligiblePlayers.Except(selectedPlayers), upcomingDuels);

            if (opponent != null) {
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
                selectedPlayers.Add(player); // Hinzufügen des Spielers zur Liste der ausgewählten Spieler
                selectedPlayers.Add(opponent); // Hinzufügen des Gegners zur Liste der ausgewählten Spieler
            }
        }
        return Ok(upcomingDuels);
    }
    */
    /*
    private Player FindBestOpponent(Player player, IEnumerable<Player> eligiblePlayers,
        IEnumerable<Duel> upcomingDuels) {
        var availableOpponents = eligiblePlayers.Where(p => p.Id != player.Id && !IsPlayerSelected(p, upcomingDuels));
        availableOpponents = availableOpponents.OrderBy(p => Math.Abs(p.EloRating - player.EloRating));
        return availableOpponents.FirstOrDefault();
    }
    */
    /*
    private Player FindBestOpponent(Player player, IEnumerable<Player> eligiblePlayers, IEnumerable<Duel> upcomingDuels) {
        var availableOpponents = eligiblePlayers
            .Where(p => p.Id != player.Id && !IsPlayerSelected(p, upcomingDuels))
            .OrderBy(p => Math.Abs(p.EloRating - player.EloRating));

        return availableOpponents.FirstOrDefault();
    }
    private bool IsPlayerSelected(Player player, IEnumerable<Duel> upcomingDuels) {
        return upcomingDuels.Any(duel => duel.Player1.Id == player.Id || duel.Player2.Id == player.Id);
    }
    */
}