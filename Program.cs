using DiskUsageAnalyzer.Engine;
using DiskUsageAnalyzer.Models;
using Spectre.Console;

namespace DiskUsageAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = "/";
            
            DiskAttributes results;

            results = DiskAnalyzer.Scan(path);

            AnsiConsole.Markup($"[green]Total size occupied:[/] {results.TotalSizeOccupiedInBytes} Bytes\n");
            AnsiConsole.Markup($"[green]Total files visited:[/] {results.TotalFilesVisited} Bytes\n");

        }
    }
}

