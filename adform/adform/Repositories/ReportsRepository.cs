using adform.Models;
using adform.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace adform.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ReportsRepository(
            IHttpClientFactory clientFactory
            )
        {
            _clientFactory = clientFactory;
        }

        public async Task<ReportResult> GetReports(string token)
        {
            var url = "https://api.adform.com/v1/reportingstats/publisher/reportdata";
            var httpClient = _clientFactory.CreateClient();
            Report report = new Report
            {
                filter = new Filters { date = "lastYear"},
                dimensions = new string[]{ "date"},
                metrics = new string[] { "bidRequests" }
            };

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var requestResult = httpClient.PostAsJsonAsync(url, report);

            string jsonString = await requestResult.Result.Content.ReadAsStringAsync();

            ReportResult result = JsonConvert.DeserializeObject<ReportResult>(jsonString);

            return result;
        }
    }
}
