using Spectre.Console;

namespace DiskUsageAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = "/home/simone";
            long totalFileSizeInBytes = 0;
            long fileVisitedCount = 0;

            

            try
            {

                // Se la cartella non esiste lancia un eccezione
                if(!Directory.Exists(path)) throw new DirectoryNotFoundException();

                Console.WriteLine($"Directory: {path}");
                
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                var files = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories);

                AnsiConsole.Status().Start(
                    "Analyzing subdirecotries...",
                    ctx =>
                    {
                        // per ogni file lo conta e somma la dimensione a quella totale
                        foreach (FileInfo file in files)
                        {
                            totalFileSizeInBytes += file.Length; 
                            fileVisitedCount++;
                        }

                        AnsiConsole.Markup("[green]Operation completed[/]\n");
                    }
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(totalFileSizeInBytes);
            Console.WriteLine(fileVisitedCount);
        }
    }
}

