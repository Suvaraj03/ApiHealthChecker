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
                using var request = new HttpRequestMessage(HttpMethod.Get, result.Url);
                Stopwatch stopwatch = Stopwatch.StartNew();
                var response = await _httpClient.SendAsync(request);
                stopwatch.Stop();
                var body = await response.Content.ReadAsStringAsync();
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.StatusCode = (int)response.StatusCode;
                result.IsHealthy = response.IsSuccessStatusCode;
                result.ResponseBody = body.Length > 300 ? body.Substring(0, 300) + "..." : body;
                result.Message = response.IsSuccessStatusCode ? "OK" : $"HTTP {response.StatusCode}";
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
