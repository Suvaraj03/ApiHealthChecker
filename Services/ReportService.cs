using ApiHealthChecker.Models;
using System.Text.Json;

namespace ApiHealthChecker.Services
{
    public class ReportService
    {
        private readonly string reportFolder = @"C:\Reports";
        public async Task SaveAsync(HealthResults results, string? environment = null)
        {
            string folderpath = reportFolder;
            if(!string.IsNullOrEmpty(environment))
            {
                folderpath = Path.Combine(reportFolder, environment);
            }
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            var servicename = GetServiceName(results.Url);
            var fileName = $"health_report_{servicename}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(folderpath, fileName);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(results, options);
            await File.WriteAllTextAsync(filePath,json);
            Console.WriteLine($"Report saved:{filePath}");
        }
        private string GetServiceName(string url)
        {
            try
            {
                var uri = new Uri(url);
                var domain = uri.Host.Replace("www.", "").Split('.')[0]; //domain
                var path = uri.AbsolutePath.Trim('/').Replace("/", "_");  //endpoint
                if (string.IsNullOrEmpty(path))
                {
                    path = "root";
                }
                return $"{domain}_{path}";
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
