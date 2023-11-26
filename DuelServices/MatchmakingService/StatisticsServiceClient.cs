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
}