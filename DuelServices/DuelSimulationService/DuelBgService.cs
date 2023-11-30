using PlayerStatisticsService;
using System.Text.Json;
using MatchmakingService;

namespace DuelSimulation;

public class DuelBgService : BackgroundService {
    private static HttpClient _httpClient = new HttpClient();
    List<Duel> upcomingDuels = new List<Duel>();
    private MatchmakingServiceClient _matchmakingServiceClient = new MatchmakingServiceClient(_httpClient);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            upcomingDuels = await _matchmakingServiceClient.GetMatchedPlayers();
            foreach (var duel in upcomingDuels) {
                int outcome = SimulateDuelOutcome();
                int eloDelta = CalculateEloDelta(duel.Player1.EloRating, duel.Player2.EloRating);

                Console.WriteLine(eloDelta);

                if (outcome == 1) // Player 1 wins
                {
                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating + eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating - eloDelta);
                    UpdateStatistics(duel); //duel.Player1, duel.Player2, outcome
                }
                else if (outcome == 2) // Player 2 wins
                {
                    UpdateEloRating(duel.Player1.Id, duel.Player1.EloRating - eloDelta);
                    UpdateEloRating(duel.Player2.Id, duel.Player2.EloRating + eloDelta);
                    UpdateStatistics(duel); //duel.Player1, duel.Player2, outcome
                }
                else if (outcome == 0) // No change in Elo ratings for a draw
                {
                    UpdateStatistics(duel);
                }
                Console.WriteLine(duel.Player1.Name + " vs " + duel.Player2.Name );
                await Task.Delay(10_000);
            }
          //  await Task.Delay(10_000);
        }
    }
    
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
        await _httpClient.PutAsync($"http://localhost:5099/Registration/UpdateEloRating/{playerId}/{neweloDelta}", null);
    }
    private async Task UpdateStatistics(Duel duel)
    {
        Console.WriteLine("hier");
        await _httpClient.PostAsJsonAsync($"http://localhost:5150/Statistics/UpdatePlayerStatistics", duel);
    }
}