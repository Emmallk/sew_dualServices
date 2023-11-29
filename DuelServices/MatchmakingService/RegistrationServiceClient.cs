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
}