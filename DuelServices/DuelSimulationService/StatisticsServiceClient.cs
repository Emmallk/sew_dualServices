using DuelSimulation;
using PlayerStatisticsService;
using RegistrationService;

namespace MatchmakingService;

public class StatisticsServiceClient
{
    private readonly HttpClient httpClient;

    public StatisticsServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dictionary<int, PlayerStatistics>> GetPlayerStatistics()
    {
        var response = await httpClient.GetFromJsonAsync<Dictionary<int, PlayerStatistics>>("/Statistics");
        return response ?? new Dictionary<int, PlayerStatistics>();
    }
    
    public async Task UpdateGeneralStatistics(Player player1, Player player2, int outcome)
    {
        var response = await httpClient.PostAsJsonAsync("/Statistics/UpdateGeneralStatistics", new { player1, player2, outcome });
        response.EnsureSuccessStatusCode();
    }


}