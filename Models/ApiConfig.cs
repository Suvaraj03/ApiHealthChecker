using System;
using System.Collections.Generic;
using System.Text;

namespace ApiHealthChecker.Models
{
    public class ApiConfig
    {
        public List<ServiceConfig> Services { get; set; } = new();
    }
    public class ServiceConfig
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
