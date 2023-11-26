using System.Net.Http.Json;

namespace DuelSimulation;

public class MatchmakingServiceClient
{
    private readonly HttpClient httpClient;

    public MatchmakingServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<Duel>> GetMatchmaking()
    {
        var response = await httpClient.GetAsync("/Matchmaking");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<List<Duel>>();
    }
}