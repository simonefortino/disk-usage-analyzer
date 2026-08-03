using DiskUsageAnalyzer.Engine;
using Spectre.Console;

namespace DiskUsageAnalyzer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string path = "/";

            var results = DiskAnalyzer.Scan(path);

            AnsiConsole.Markup($"[green]Total size occupied:[/] {results.TotalSizeOccupiedInBytes} Bytes\n");
            AnsiConsole.Markup($"[green]Total files visited:[/] {results.TotalFilesVisited}\n");

        }
    }
}

