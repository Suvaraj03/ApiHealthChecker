using ApiHealthChecker.Models;
using System.Diagnostics;

namespace ApiHealthChecker.Services
{
    public class HealthChecker
    {
        private readonly HttpClient _httpClient;
        public HealthChecker()
        {
            _httpClient = new HttpClient();
        }
        public async Task<HealthResults> GetHealthResultsAsync(string url,string name)
        {
            var result = new HealthResults();
            result.Name = name;
            result.Url = url;
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var response = await _httpClient.GetAsync(url);
                stopwatch.Stop();
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.StatusCode = (int)response.StatusCode;
                result.IsHealthy = response.IsSuccessStatusCode;
                result.Message = response.IsSuccessStatusCode ? "API is working" : $"API returned error {response.StatusCode}";
            }
            catch (Exception ex)
            {
                result.IsHealthy = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
