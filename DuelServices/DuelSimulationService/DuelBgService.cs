using PlayerStatisticsService;
using System.Text.Json;
using MatchmakingService;
using Duel = MatchmakingService.Duel;

namespace DuelSimulation;

public class DuelBgService : BackgroundService {
    
    private static HttpClient _httpClient = new HttpClient();
    List<Duel> upcomingDuels = new List<Duel>();
    private MatchmakingServiceClient _matchmakingServiceClient = new MatchmakingServiceClient(_httpClient);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        
        while (!stoppingToken.IsCancellationRequested) {
            
            //Holt sich die Liste der Upcoming Duels aus dem Matchmaking Service
            upcomingDuels = await _matchmakingServiceClient.GetMatchedPlayers();
            
            foreach (var duel in upcomingDuels) {
                
                duel.DuelResult = SimulateDuelOutcome();
                duel.LastDuel = DateTime.Now; //Duel wird auf jetzigen Zeitstand gestellt
                
                //Elo berechnen
                int eloDelta = CalculateEloDelta(duel.Player1.EloRating, duel.Player2.EloRating);

                if ( duel.DuelResult == 1) // Player 1 gewinnt
                {
                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating + eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating - eloDelta);

                    duel.Player1.EloRating = duel.Player1.EloRating + eloDelta;
                    duel.Player2.EloRating = duel.Player2.EloRating - eloDelta;

                    
                    UpdateStatistics(duel);
                }
                else if ( duel.DuelResult == 2) // Player 2 gewinnt
                {

                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating - eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating + eloDelta);
                    
                    duel.Player1.EloRating = duel.Player1.EloRating - eloDelta;
                    duel.Player2.EloRating = duel.Player2.EloRating + eloDelta;
                    
                    UpdateStatistics(duel); 
                }
                else if ( duel.DuelResult == 0) // Unentschieden (Elo verändert sich nicht)
                {
                    UpdateStatistics(duel);
                }
                Console.WriteLine(duel.Player1.Name + " vs " + duel.Player2.Name );
                await Task.Delay(10_000);
            }
        }
    }
    
    //Ergebnis des Duells wird zufällig entschieden (1/3 Chance)
    private int SimulateDuelOutcome()
    {
        Random random = new Random();
        int outcome = random.Next(0, 3);
        return outcome;
    }

    private double ExpectationToWin(int playerOneRating, int playerTwoRating)
    {
        return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
    } 
    private int CalculateEloDelta(int playerOneRating, int playerTwoRating)
    {
        int eloK = 32;
        return (int)(eloK * (1 - ExpectationToWin(playerOneRating, playerTwoRating)));
    }
    
    private async Task UpdateEloRating(int playerId, int neweloDelta)
    {
        Console.WriteLine(playerId + " " + neweloDelta);
        await _httpClient.PutAsync($"http://localhost:5254/Registration/UpdateEloRating/{playerId}/{neweloDelta}", null);
    }   //await _httpClient.PutAsync($"http://localhost:5099/Registration/UpdateEloRating/{playerId}?eloDelta={neweloDelta}&name={name}", null);
    private async Task UpdateStatistics(Duel duel)
    {
        await _httpClient.PostAsJsonAsync($"http://localhost:5150/Statistics/UpdatePlayerStatistics", duel);
    }
}