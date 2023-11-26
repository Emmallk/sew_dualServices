using DuelSimulation;
using PlayerStatisticsService;

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
    
    public async Task UpdateGeneralStatistics(Duel duel, int result)
    {
        var response = await httpClient.PostAsJsonAsync("/Statistics/UpdateGeneralStatistics", new { duel, result });
        response.EnsureSuccessStatusCode();
    }
}