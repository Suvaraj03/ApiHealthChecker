using ApiHealthChecker.Services;
using ApiHealthChecker.Utils;
using Spectre.Console;
//Console.Write("Enter API URL: ");
//string? url = Console.ReadLine();
if(args.Length == 0)
{
    AnsiConsole.MarkupLine("[red]Usage: health-check <url>[/]");
    return;
}
string url = args[0];
var checker = new HealthChecker();
var result = await checker.GetHealthResultsAsync(url);
if (result.IsHealthy)
{
    ConsoleHelper.Success("API Healthy");
}
else
{
    ConsoleHelper.Failure("API Failed");
}
Console.WriteLine();
Console.WriteLine($"URL : {result.Url}");
Console.WriteLine($"Status Code : {result.StatusCode}");
Console.WriteLine($"Response Time : {result.ResponseTime} ms");
Console.WriteLine($"Message : {result.Message}");