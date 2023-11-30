using System.Text.Json;
using RegistrationService;

namespace MatchmakingService;

public class RegistrationServiceClient
{
    private readonly HttpClient _httpClient;

    public RegistrationServiceClient(HttpClient _httpClient)
    {
        this._httpClient = _httpClient;
    }

    public async Task<List<Player>> GetPlayersFromRemoteService() {
        //Get auf die Players Liste von RegistrationService über die URL (Port, etc.)
        List<Player> players = await _httpClient.GetFromJsonAsync<List<Player>>("http://localhost:5254/Registration/Players");
        return players;
    }
}