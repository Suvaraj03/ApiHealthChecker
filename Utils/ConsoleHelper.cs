using Spectre.Console;

namespace ApiHealthChecker.Utils
{
    public static class ConsoleHelper
    {
        public static void Success(string message)
        {
            AnsiConsole.MarkupLine("[green]\u2714 Api Healthy[/]");//unicode refers to ✔
        }
        public static void Failure(string message) 
        {
            AnsiConsole.MarkupLine("[red]\u2718 Api Failed"); //unicode refers to ✘
        }
    }
}
