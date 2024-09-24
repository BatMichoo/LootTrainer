using System.Net.Http.Headers;

namespace Core.Services.BattleNet
{
    public class BattleNetService
    {
        private readonly HttpClient _client;

        public BattleNetService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> Test(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync(Endpoints.Profile.Summary + "?locale=en_GB");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
