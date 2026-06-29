using ApiHealthChecker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace ApiHealthChecker.Services
{
    public class HealthChecker
    {
        private readonly HttpClient _httpClient;
        public HealthChecker()
        {
            _httpClient = new HttpClient();
        }
        public async Task<HealthResults> GetHealthResultsAsync(string url)
        {
            var result = new HealthResults();
            result.Url = url;
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var response = await _httpClient.GetAsync(url);
                stopwatch.Stop();
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.StatusCode =(int)response.StatusCode;
                result.IsHealthy = response.IsSuccessStatusCode;
                result.Message = response.IsSuccessStatusCode? "API is working": "API returned error";
            }
            catch(Exception ex) 
            {
                result.IsHealthy =false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
