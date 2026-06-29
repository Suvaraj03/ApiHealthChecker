namespace ApiHealthChecker.Models
{
    public class HealthResults
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public bool IsHealthy { get; set; }
        public int StatusCode { get; set; }
        public long ResponseTime { get; set; }
        public string Message { get; set; } = "";
        public string? ResponseBody { get; set; }

    }
}
