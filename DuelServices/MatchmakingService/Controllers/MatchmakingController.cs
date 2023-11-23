using MatchmakingService.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        // Get registered players from the Registration Service
        List<Player> registeredPlayers = _registrationService.GetRegisteredPlayers();

        // Get upcoming duels
        List<Duel> upcomingDuels = GetUpcomingDuels(registeredPlayers);

        // Order duels by priority (first-time players or players with a long gap since their last duel)
        upcomingDuels = OrderDuelsByPriority(upcomingDuels);

        return Ok(upcomingDuels);
    }

    private List<Duel> GetUpcomingDuels(List<Player> players) {
        List<Duel> upcomingDuels = new List<Duel>();

        // Logic to match players based on Elo values
        // You need to implement your own logic here based on your requirements

        // Example: Matching players with similar Elo values
        foreach (var player in players)
        {
            var opponent = players.Where(p => p.Id != player.Id)
                .OrderBy(p => Math.Abs(p.Elo - player.Elo))
                .FirstOrDefault();

            if (opponent != null) {
                upcomingDuels.Add(new Duel { Player1 = player, Player2 = opponent });
            }
        }
        return upcomingDuels;
    }

    private List<Duel> OrderDuelsByPriority(List<Duel> duels)
    {
        // Logic to order duels by priority (first-time players or players with a long gap since their last duel)
        // You need to implement your own logic here based on your requirements

        // Example: Ordering by players who never played a duel or have a long gap since the last duel
        duels = duels.OrderBy(d =>d.Player1.LastPlayedDuel == null)
            .ThenBy(d => d.Player2.LastPlayedDuel == null)
            .ToList();
           /*
                d.Player1.LastPlayedDuel == null || (DateTime.Now - d.Player1.LastPlayedDuel).Days > 30)
            .ThenBy(d => d.Player2.LastPlayedDuel == null || (DateTime.Now - d.Player2.LastPlayedDuel).Days > 30)
            .ToList();
            */
        return duels;
    }
}


public interface IStatisticsService
{
    // Add methods to retrieve statistics, such as last played duel date
}

public class Player
{
    public int Id { get; set; }
    public int Elo { get; set; }
    public DateTime? LastPlayedDuel { get; set; }
}

public class Duel
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
}
