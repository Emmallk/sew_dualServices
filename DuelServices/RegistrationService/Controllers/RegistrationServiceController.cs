using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RegistrationService.Controllers;

[ApiController]
[Route("Registration")]
public class RegistrationServiceController : Controller {
    private static List<Player> players = new List<Player>();

    //Endpunkt GET/Players
    [HttpGet("Players")]
    public IActionResult GetAllPlayers() {
        return Ok(players);
    }
    
    //Einfügen einer Liste mit eigenen EloRating
    /*[HttpPost("RegistrationListWithElo")]
    public IActionResult CreatePlayers([FromBody] List<Player> newPlayers) {
        if (newPlayers == null || newPlayers.Count == 0) {
            return BadRequest("Invalid player data");
        }
        foreach (var newPlayer in newPlayers) {
            newPlayer.EloRating = newPlayer.EloRating == 0 ? 1500 : newPlayer.EloRating;
            newPlayer.Id = players.Count + 1;
            players.Add(newPlayer);
        }
        return CreatedAtAction(nameof(GetAllPlayers), newPlayers);
    }
    */ 
    
    //Einfügen einer Liste von Spielern nur mit Namen
    [HttpPost("RegistrationList")]
    public IActionResult CreatePlayers([FromBody] List<string> playerNames)
    {
        if (playerNames == null || playerNames.Count == 0)
        {
            return BadRequest("Invalid player data");
        }
        foreach (var name in playerNames)
        {
            Player newPlayer = new Player
            {
                Name = name,
                EloRating = 1500,
                Id = players.Count + 1 
            };
            players.Add(newPlayer);
        }
        return CreatedAtAction(nameof(GetAllPlayers), playerNames);
    }
    
    //Endpunkt POST /Registration
    [HttpPost("Registration")]
    public IActionResult CreatePlayer([FromBody] string playerName)
    {
        //Neuer Player = Elo automatisch 1500
        Player newPlayer = new Player
        {
            Name = playerName,
            EloRating = 1500, 
            Id = players.Count + 1 
        };
        players.Add(newPlayer);
        return CreatedAtAction(nameof(GetAllPlayers), new { id = newPlayer.Id }, playerName);
    }
    
    //Endpunkt PUT /Registration/{id}
    [HttpPut("UpdateEloRating/{playerId}/{eloDelta}")]
    public IActionResult UpdateEloRating(int playerId, int eloDelta) {
        Player player = players.FirstOrDefault(p => p.Id == playerId);

        if (player != null) {
            player.EloRating = eloDelta;
            return Ok();
        }
        else {
            return NotFound($"Player with ID {playerId} not found.");
        }
    }
}