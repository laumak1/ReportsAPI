using adform.Models;
using adform.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace adform.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthorizationRepository(
            IHttpClientFactory clientFactory
            )
        {
            _clientFactory = clientFactory;
        }

        public async Task<ReceivedToken> Authorize(string id, string secret)
        {
            var url = "https://id.adform.com/sts/connect/token";
            var httpClient = _clientFactory.CreateClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", id),
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("scope", "https://api.adform.com/scope/eapi")
            });

            var requestResult = httpClient.PostAsync(url, content);

            string jsonString = await requestResult.Result.Content.ReadAsStringAsync();

            ReceivedToken result = JsonConvert.DeserializeObject<ReceivedToken>(jsonString);

            return result;
        }
    }
}
