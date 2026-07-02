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
    Console.WriteLine("health-check <url> --verbose");
    Console.WriteLine("health-check -c <config.json> --verbose");
    Console.WriteLine("health-check <url> --save"); 
    Console.WriteLine("health-check -c <config.json> --save");
    return;
}
string input = args[0];
bool isVerbose = args.Contains("--verbose") || args.Contains("-v");
var checker = new HealthChecker();
var reportService = new ReportService();
bool saveReport = args.Contains("--save") || args.Contains("-s");
//
// Single URL Check
//
if (Uri.TryCreate(input, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)) 
{
    var result = await checker.GetHealthResultsAsync(input, "API");
    if (saveReport)
    {
        var report = new HealthReport
        {
            GeneratedAt = DateTime.Now,
            Total = 1,
            Healthy = result.IsHealthy ? 1 : 0,
            Failed = result.IsHealthy ? 0 : 1,
            Results = new List<HealthResults>
            {
                result
            }
        };
        await reportService.SaveAsync(result);
       

    }
    if (isVerbose)
    {
        AnsiConsole.MarkupLine("[bold yellow]--- VERBOSE MODE ---[/]");
        Console.WriteLine($"URL           : {result.Url}");
        Console.WriteLine($"Name          : {result.Name}");
        Console.WriteLine($"Status Code   : {result.StatusCode}");
        Console.WriteLine($"Response Time : {result.ResponseTime} ms");
        Console.WriteLine($"Success       : {result.IsHealthy}");
        Console.WriteLine($"Message       : {result.Message}");
        Console.WriteLine($"Response Body : {result.ResponseBody}");
        Console.WriteLine("----------------------");
        return;
    }
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

    try
    {
        if (!File.Exists(configPath))
        {
            ConsoleHelper.Failure(
                $"Config file not found: {configPath}"
            );
            return;
        }
        var json = await File.ReadAllTextAsync(configPath);
        var config = JsonSerializer.Deserialize<ApiConfig>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (config == null)
        {
            ConsoleHelper.Failure("Invalid config file");
            return;
        }

        if (config.Services == null || config.Services.Count == 0)
        {
            ConsoleHelper.Failure("No services found in config file");
            return;
        }

        AnsiConsole.MarkupLine("[yellow]Checking Services...[/]");

        int healthy = 0;
        int failed = 0;

        string environment = Path.GetFileNameWithoutExtension(configPath);


        foreach (var service in config.Services)
        {
            try
            {
                var result =
                    await checker.GetHealthResultsAsync(
                        service.Url,
                        service.Name
                    );
                if (result.IsHealthy)
                    healthy++;
                else
                    failed++;
                if (saveReport)
                {
                    await reportService.SaveAsync(result, environment);
                }
                if (!isVerbose)
                {
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
                }
                else
                {
                    AnsiConsole.WriteLine("\n------------------------");
                    AnsiConsole.WriteLine($"SERVICE: {result.Name}");
                    AnsiConsole.WriteLine("--------------------------");

                    AnsiConsole.WriteLine($"URL: {result.Url}");
                    AnsiConsole.WriteLine("Method: GET");
                    AnsiConsole.WriteLine($"Time: {result.ResponseTime}ms");
                    AnsiConsole.WriteLine($"Status: {result.StatusCode}");
                    AnsiConsole.WriteLine($"Success: {result.IsHealthy}");
                }
            }
            catch (Exception ex)
            {
                failed++;

                ConsoleHelper.Failure(
                    $"Error checking {service.Name}: {ex.Message}"
                );
            }
        }

        Console.WriteLine();
        Console.WriteLine("Summary");
        Console.WriteLine($"Total : {config.Services.Count}");
        Console.WriteLine($"Healthy : {healthy}");
        Console.WriteLine($"Failed : {failed}");

        return;
    }
    catch (JsonException jsonEx)
    {
        ConsoleHelper.Failure($"Invalid JSON format: {jsonEx.Message}");
        return;
    }
    catch (IOException ioEx)
    {
        ConsoleHelper.Failure($"File read error: {ioEx.Message}");
        return;
    }
    catch (Exception ex)
    {
        ConsoleHelper.Failure($"Unexpected error: {ex.Message}");
        return;
    }
}
Console.WriteLine("Invalid URL or config file");
