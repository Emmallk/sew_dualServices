using System.Text.Json;
using PlayerStatisticsService;

namespace MatchmakingService;

public class StatisticsServiceClient
{
    private readonly HttpClient httpClient;

    public StatisticsServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /*
    public async Task<Dictionary<int, PlayerStatistics>> GetPlayerStatistics()
    {
        var response = await httpClient.GetFromJsonAsync<Dictionary<int, PlayerStatistics>>("/Statistics");
        return response ?? new Dictionary<int, PlayerStatistics>();
    }
    */
    
    public List<PlayerStatistics> GetPlayerStatisticsFromRemoteService() {
        using (var httpClient = new HttpClient()) {
            HttpResponseMessage response =
                httpClient.GetAsync("http://localhost:5099/Statistics/GetAllPlayerStatistics").Result;

            if (response.IsSuccessStatusCode) {
                string content = response.Content.ReadAsStringAsync().Result;
                List<PlayerStatistics> playerStatistics = JsonSerializer.Deserialize<List<PlayerStatistics>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return playerStatistics;
            }

            return null;
        }
    }
}