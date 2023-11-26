using RegistrationService;
using RegistrationService.Controllers;

public interface IRegistrationService
{
     List<Player> GetRegisteredPlayers();
}