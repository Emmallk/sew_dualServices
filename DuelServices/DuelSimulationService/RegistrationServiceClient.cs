using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RegistrationService;

namespace DuelSimulation
{
    public class RegistrationServiceClient
    {
        private readonly HttpClient httpClient;

        public RegistrationServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            var response = await httpClient.GetAsync("/Registration");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Player>>();
        }

        public async Task<Player> CreatePlayer(Player newPlayer)
        {
            var response = await httpClient.PostAsJsonAsync("/Registration", newPlayer);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Player>();
        }

        public async Task UpdatePlayer(int id, Player updatedPlayer)
        {
            var response = await httpClient.PutAsJsonAsync($"/Registration/{id}", updatedPlayer);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateElo(int playerId, int newElo)
        {
            var eloUpdate = new { Id = playerId, Elo = newElo };
            var response = await httpClient.PutAsJsonAsync("/Registration/UpdateElo", eloUpdate);
            response.EnsureSuccessStatusCode();
        }
    }
}