using PlayerStatisticsService;

namespace DuelSimulation;
using System.Text.Json;

public class DuelBgService : BackgroundService {
    private static HttpClient _httpClient = new HttpClient();
    List<Duel> upcomingDuels = new List<Duel>();
    private MatchmakingServiceClient _matchmakingServiceClient = new MatchmakingServiceClient(_httpClient);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        upcomingDuels = await _matchmakingServiceClient.GetMatchedPlayers();
        while (!stoppingToken.IsCancellationRequested) {
            foreach (var duel in upcomingDuels) {
                int outcome = SimulateDuelOutcome();
                int eloDelta = CalculateEloDelta(duel.Player1.EloRating, duel.Player2.EloRating);

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
            }

            await Task.Delay(10_000);
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
        await _httpClient.PutAsync($"http://localhost:5098/Registration/UpdateEloRating/{playerId}/{neweloDelta}",
            null);
    }
    private async Task UpdateStatistics(Duel duel)
    {
        await _httpClient.PutAsync($"http://localhost:5100/Statistics/UpdatePlayerStatistics",
            new StringContent(JsonSerializer.Serialize(duel)));
    }

}