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

    [HttpGet("Players")]
    public IActionResult GetAllPlayers() {
        return Ok(players);
    }

    /*
    [HttpPost("RegistrationList")]
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
    
    [HttpPost("Registration")]
    public IActionResult CreatePlayer([FromBody] string playerName)
    {
        Player newPlayer = new Player
        {
            Name = playerName,
            EloRating = 1500,
            Id = players.Count + 1 
        };
        players.Add(newPlayer);
        return CreatedAtAction(nameof(GetAllPlayers), new { id = newPlayer.Id }, playerName);
    }
    
   
    // TODO: Name und Elo-Wert können aktualisiert werden
    
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

    /*
    [HttpPut("UpdatePlayerlist")]
    public IActionResult UpdatePlayers([FromBody] List<Player> updatedPlayers) {
        if (updatedPlayers == null || updatedPlayers.Count == 0) {
            return BadRequest("Invalid player data");
        }

        foreach (var updatedPlayer in updatedPlayers) {
            if (string.IsNullOrWhiteSpace(updatedPlayer.Name)) {
                return BadRequest("Invalid player data");
            }

            var existingPlayer = players.FirstOrDefault(p => p.Id == updatedPlayer.Id);
            if (existingPlayer == null) {
                return NotFound($"Player with ID {updatedPlayer.Id} not found");
            }

            existingPlayer.Name = updatedPlayer.Name;
            existingPlayer.EloRating = updatedPlayer.EloRating;
        }

        return Ok(updatedPlayers);
    }

    [HttpPut("UpdatePlayer/{id}")]
    public IActionResult UpdatePlayer(int id, [FromBody] Player updatedPlayer) {
        if (updatedPlayer == null || string.IsNullOrWhiteSpace(updatedPlayer.Name) || id != updatedPlayer.Id) {
            return BadRequest("Invalid player data");
        }

        var existingPlayer = players.FirstOrDefault(p => p.Id == id);

        if (existingPlayer == null) {
            return NotFound();
        }

        existingPlayer.Name = updatedPlayer.Name;
        existingPlayer.EloRating = updatedPlayer.EloRating;

        return Ok(existingPlayer);
    }
    */
}