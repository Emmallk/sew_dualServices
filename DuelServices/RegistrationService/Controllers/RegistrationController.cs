using Microsoft.AspNetCore.Mvc;
using RegistrationService.Service;
/*
namespace RegistrationService.Controllers;

[Route("api/[controller]")]
//[Route("Registration")]
[ApiController]
public class RegistrationController : ControllerBase {
    private readonly PlayerService _playerService;
    public RegistrationController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    public ActionResult<Player> CreatePlayer([FromBody] string name)
    {
        var newPlayer = _playerService.AddPlayer(name);
        return Ok(newPlayer);
    }


    [HttpPost("register")]
    public IActionResult RegisterPlayer([FromBody] string playerName)
    {
        Player newPlayer = _playerService.AddPlayer(playerName);
        return Ok(newPlayer);
    }


    [HttpPut("{id}")]
    public ActionResult<Player> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
    {
        var player = _playerService.UpdatePlayer(id, updatedPlayer.Name, updatedPlayer.EloRating);
        if (player == null)
        {
            return NotFound();
        }
        return Ok(player);
    }


    [HttpGet]
    public ActionResult<List<Player>> GetPlayers()
    {
        var players = _playerService.GetPlayers();
        return Ok(players);
    }

}
*/