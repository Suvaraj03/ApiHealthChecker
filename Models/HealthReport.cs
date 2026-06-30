namespace ApiHealthChecker.Models
{
    public class HealthReport
    {
        public DateTime GeneratedAt { get; set; }
        public int Total { get; set; }
        public int Healthy { get; set; }
        public int Failed { get; set; }
        public List<HealthResults> Results { get; set; }

    }
}
