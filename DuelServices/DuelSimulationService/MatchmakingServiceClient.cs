using System.Net.Http.Json;
using System.Text.Json;
using MatchmakingService;

namespace DuelSimulation;

public class MatchmakingServiceClient
{
    private readonly HttpClient _httpClient;
    List<Duel> upcomingDuels = new List<Duel>();

    public MatchmakingServiceClient(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    public async Task<List<Duel>> GetMatchedPlayers() {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5099/Matchmaking/Matchmaking");
        if (response.IsSuccessStatusCode) {
            string content = await response.Content.ReadAsStringAsync();
            upcomingDuels = JsonSerializer.Deserialize<List<Duel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return upcomingDuels;
        }
        return null;
    }
}