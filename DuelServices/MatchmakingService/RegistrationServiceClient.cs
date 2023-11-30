using System.Text.Json;
using RegistrationService;

namespace MatchmakingService;

public class RegistrationServiceClient
{
    private readonly HttpClient httpClient;

    public RegistrationServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    

    public async Task<List<Player>> GetPlayers()
    {
        var response = await httpClient.GetFromJsonAsync<List<Player>>("/Registration");
        return response ?? new List<Player>();
    }
    public List<Player> GetPlayersFromRemoteService() {
        using (var httpClient = new HttpClient()) {
            HttpResponseMessage response =
                httpClient.GetAsync("http://localhost:5099/Registration/GetAllPlayers").Result;

            if (response.IsSuccessStatusCode) {
                string content = response.Content.ReadAsStringAsync().Result;
                List<Player> players = JsonSerializer.Deserialize<List<Player>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return players;
            }
            return null;
        }
    }
    
}