using ApiHealthChecker.Models;
using ApiHealthChecker.Services;
using ApiHealthChecker.Utils;
using Spectre.Console;
using System.Text.Json;
Console.OutputEncoding = System.Text.Encoding.UTF8;
if (args.Length == 0)
{
    AnsiConsole.MarkupLine("[red]Usage:[/]");
    Console.WriteLine("health-check <url>");
    Console.WriteLine("health-check -c <config.json>");
    return;
}
string input = args[0];
var checker = new HealthChecker();
//
// Single URL Check
//
if (Uri.TryCreate(input, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
{
    var result = await checker.GetHealthResultsAsync(input, "API");
    if (result.IsHealthy)
    {
        ConsoleHelper.Success(
            $"{result.Name} - {result.StatusCode} ({result.ResponseTime}ms)"
        );
    }
    else
    {
        ConsoleHelper.Failure(
            $"{result.Name} - {result.Message}"
        );
    }
    return;

}

//
// Multiple URL Check
//
if (args.Length >= 2 &&
    (args[0] == "--config" || args[0] == "-c"))
{
    string configPath = args[1];
    if (!File.Exists(configPath))
    {
        ConsoleHelper.Failure(
            $"Config file not found: {configPath}"
        );
        return;
    }
    var json = await File.ReadAllTextAsync(configPath);
    var config =JsonSerializer.Deserialize<ApiConfig>(json);
    if (config == null)
    {
        ConsoleHelper.Failure(
            "Invalid config file"
        );
        return;
    }
    AnsiConsole.MarkupLine(
        "[yellow]Checking Services...[/]"
    );
    int healthy = 0;
    int failed = 0;
    foreach (var service in config.Services)
    {
        var result =
            await checker.GetHealthResultsAsync(
                service.Url,
                service.Name
            );
        if (result.IsHealthy)
        {
            ConsoleHelper.Success(
                $"{result.Name} - {result.StatusCode} ({result.ResponseTime}ms)"
            );
            healthy++;
        }
        else
        {
            ConsoleHelper.Failure(
                $"{result.Name} - {result.Message}"
            );

            failed++;
        }

    }
    Console.WriteLine();
    Console.WriteLine("Summary");
    Console.WriteLine($"Total : {config.Services.Count}");
    Console.WriteLine($"Healthy : {healthy}");
    Console.WriteLine($"Failed : {failed}");

    return;
}
Console.WriteLine("Invalid URL or config file");
