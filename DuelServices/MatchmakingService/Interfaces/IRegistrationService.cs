using MatchmakingService.Controllers;

namespace MatchmakingService.Interfaces;

public interface IRegistrationService
{
    List<Player> GetRegisteredPlayers();
}