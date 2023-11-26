using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RegistrationService.Controllers;

[Route("Registration")]
public class RegistrationServiceController : Controller
{
    private static List<Player> players = new List<Player>();

    // POST /Registration
    [HttpPost]
    public IActionResult CreatePlayer([FromBody] Player newPlayer)
    {
        if (newPlayer == null || string.IsNullOrWhiteSpace(newPlayer.Name))
        {
            return BadRequest("Invalid player data");
        }
        newPlayer.EloRating = newPlayer.EloRating == 0 ? 1500 : newPlayer.EloRating;
        newPlayer.Id = GenerateUniqueId();
        players.Add(newPlayer);
        return CreatedAtAction(nameof(GetAllPlayers), new { id = newPlayer.Id }, newPlayer);
    }

    // PUT /Registration/{id}
    [HttpPut("{id}")]
    public IActionResult UpdatePlayer(int id, [FromBody] Player updatedPlayer)
    {
        if (updatedPlayer == null || string.IsNullOrWhiteSpace(updatedPlayer.Name) || id != updatedPlayer.Id)
        {
            return BadRequest("Invalid player data");
        }
        var existingPlayer = players.FirstOrDefault(p => p.Id == id);

        if (existingPlayer == null)
        {
            return NotFound();
        }
        existingPlayer.Name = updatedPlayer.Name;
        existingPlayer.EloRating = updatedPlayer.EloRating;
        
        return Ok(existingPlayer);
    }

    // GET /Registration
    [HttpGet]
    public IActionResult GetAllPlayers()
    {
        return Ok(players);
    }
    private int GenerateUniqueId()
    {
        return players.Count + 1;
    }
}
