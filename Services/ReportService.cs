using ApiHealthChecker.Models;
using System.Text.Json;

namespace ApiHealthChecker.Services
{
    public class ReportService
    {
        private readonly string reportFolder = @"C:\Reports";
        public async Task SaveAsync(HealthReport report, string url)
        {
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }
            var servicename = GetServiceName(url);
            var fileName = $"health_report_{servicename}_{DateTime.Now:yyyyMMdd_HHmmss}";
            var filePath = Path.Combine(reportFolder, fileName);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(report, options);
            await File.WriteAllTextAsync(filePath,json);
            Console.WriteLine($"Report saved:{filePath}");
        }
        private string GetServiceName(string url)
        {
            try
            {
                var uri = new Uri(url);
                var host = uri.Host;
                host = host.Replace("www.", ""); //replace www
                var name = host.Split('.')[0]; //take only domain name 
                return name;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
