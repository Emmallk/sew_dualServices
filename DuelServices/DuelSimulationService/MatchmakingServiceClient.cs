using System.Net.Http.Json;
using System.Text.Json;

namespace DuelSimulation;

public class MatchmakingServiceClient
{
    private readonly HttpClient _httpClient;
    List<Duel> upcomingDuels = new List<Duel>();

    public MatchmakingServiceClient(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    /*
    public async Task<List<Duel>> GetMatchmaking()
    {
        var response = await _httpClient.GetAsync("/Matchmaking");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<List<Duel>>();
    }
    */
    public async Task<List<Duel>> GetMatchedPlayers() {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5099/Matchmaking/GetMatchmaking");
        if (response.IsSuccessStatusCode) {
            string content = await response.Content.ReadAsStringAsync();
            upcomingDuels = JsonSerializer.Deserialize<List<Duel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return upcomingDuels;
        }
        return null;
    }
}