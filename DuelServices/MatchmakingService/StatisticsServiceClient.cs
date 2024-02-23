using System.Text.Json;
using PlayerStatisticsService;

namespace MatchmakingService;

public class StatisticsServiceClient {
    private readonly HttpClient _httpClient;

    public StatisticsServiceClient(HttpClient httpClient) {
        this._httpClient = httpClient;
    }

    public async Task<List<PlayerStatistics>> GetPlayerStatisticsFromRemoteService() {
        List<PlayerStatistics> playerStatistics =
            await _httpClient.GetFromJsonAsync<List<PlayerStatistics>>(
                "http://localhost:5150/Statistics/GetAllPlayerStatistics");
        return playerStatistics;
    }
}