using RegistrationService.Interfaces;

namespace RegistrationService.Service; 

public class PlayerService : IPlayerService {
    private List<Player> players = new List<Player>();
    private int nextId = 1;

    public List<Player> GetPlayers()
    {
        return players;
    }


    /*
    public Player AddPlayer(string name)
    {
        Player newPlayer = new Player
        {
            Id = nextId++,
            Name = name,
            EloRating = 1500 
        };
        players.Add(newPlayer);
        return newPlayer;
    }

    public Player UpdatePlayer(int id, string name, int eloRating)
    {
        Player playerToUpdate = players.FirstOrDefault(p => p.Id == id);
        if (playerToUpdate != null)
        {
            playerToUpdate.Name = name;
            playerToUpdate.EloRating = eloRating;
        }
        return playerToUpdate;
    }
    */
}