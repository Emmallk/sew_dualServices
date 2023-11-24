using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MatchmakingService.Class;
using PlayerStatisticsService.Interfaces;
using RegistrationService.Class;

namespace MatchmakingService.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchmakingController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly IStatisticsService _statisticsService;

    public MatchmakingController(IRegistrationService registrationService, IStatisticsService statisticsService)
    {
        _registrationService = registrationService;
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public IActionResult GetMatchmaking()
    {
        List<Player> registeredPlayers = _registrationService.GetRegisteredPlayers();
        List<Duel> upcomingDuels = GetUpcomingDuels(registeredPlayers);
        upcomingDuels = OrderDuelsByPriority(upcomingDuels);

        return Ok(upcomingDuels);
    }

    [HttpGet]
    [Route("api/matchmaking/{playerId}")]
    public async Task<IActionResult> GetMatchmakingData(int playerId) // Ändern Sie die Methode zu async
    {
        using (HttpClient client = new HttpClient())
        {
            string playerStatisticsServiceUrl = "http://playerstatisticsservice/api/playerstatistics/" + playerId;
            var response = await client.GetAsync(playerStatisticsServiceUrl); // Ändern Sie die Methode zu async

            if (response.IsSuccessStatusCode)
            {
                //var playerStatistics =
                //    await response.Content.ReadAsAsync<PlayerStatistics>(); // Ändern Sie die Methode zu async
                
            }
            else
            {
                // Fehlerbehandlung, wenn die Anfrage nicht erfolgreich ist
                return StatusCode((int)response.StatusCode, "Fehler beim Abrufen der Spielerstatistiken");
            }
        }

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

        return upcomingDuels;
    }


    // Duell ordnen 
    private List<Duel> OrderDuelsByPriority(List<Duel> duels)
    {
        duels = duels
            .OrderBy(d =>
                (d.Player1.LastPlayedDuel == null ? DateTime.MinValue : d.Player1.LastPlayedDuel)
                >
                (d.Player2.LastPlayedDuel == null ? DateTime.MinValue : d.Player2.LastPlayedDuel))
            .ThenBy(d =>
                (d.Player1.LastPlayedDuel == null ? DateTime.MinValue : d.Player1.LastPlayedDuel)
                >
                (d.Player2.LastPlayedDuel == null ? DateTime.MinValue : d.Player2.LastPlayedDuel)
                    ? (DateTime.Now - (d.Player1.LastPlayedDuel ?? DateTime.Now)).TotalDays
                    : (DateTime.Now - (d.Player2.LastPlayedDuel ?? DateTime.Now)).TotalDays)
            .ToList();

        return duels;
    }
}

