using ApiHealthChecker.Models;
using System.Text.Json;

namespace ApiHealthChecker.Services
{
    public class ReportService
    {
        private readonly string reportFolder = @"C:\Reports";
        public async Task SaveAsync(HealthReport report, string serviceName)
        {
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }
            var safeName = string.Join("_", serviceName.Split(Path.GetRandomFileName()));
            var fileName = $"health_report_{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}";
            var filePath = Path.Combine(reportFolder, fileName);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(report, options);
            await File.WriteAllTextAsync(filePath,json);
            Console.WriteLine($"Report saved:{filePath}");
        }
    }
}
