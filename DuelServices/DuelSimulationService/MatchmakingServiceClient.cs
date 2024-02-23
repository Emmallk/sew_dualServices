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
        
        List<Duel> upcomingDuels = await _httpClient.GetFromJsonAsync<List<Duel>>("http://localhost:5099/Matchmaking/Matchmaking");
        return upcomingDuels; 
    }
}