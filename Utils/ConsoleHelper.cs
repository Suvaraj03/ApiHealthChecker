using Spectre.Console;

namespace ApiHealthChecker.Utils
{
    public static class ConsoleHelper
    {
        public static void Success(string message)
        {
            AnsiConsole.MarkupLine(
                $"[green]\u2714 {message}[/]"
            );
        }


        public static void Failure(string message)
        {
            AnsiConsole.MarkupLine(
                $"[red]\u2718 {message}[/]"
            );
        }


        public static void Warning(string message)
        {
            AnsiConsole.MarkupLine(
                $"[yellow]\u26A0 {message}[/]"
            );
        }
    }
}
