using DiskUsageAnalyzer.Models;
using Spectre.Console;


namespace DiskUsageAnalyzer.Engine
{
    public static class DiskAnalyzer
    {
        private static long _totalSizeInBytes = 0;
        private static long _totalFilesVisited = 0;

        private static DiskAttributes _diskAttributes = new DiskAttributes();

        // cartelle di sistema da escludere
        private static string[] _excludedDirectories = 
        {
            "/proc",
            "/sys",
            "/dev",
            "/run",
            "/mnt",
            "/media",
            "/lost+found"
        };

        public static DiskAttributes Scan(string path)
        {
            try
            {

                // Se la cartella non esiste lancia un eccezione
                if(!Directory.Exists(path)) throw new DirectoryNotFoundException();

                Console.WriteLine($"Directory: {path}");
                
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                AnsiConsole.Status()
                    .Start(
                    "Analyzing subdirecotries...",
                    ctx =>
                    {
                        DirectoryInfo rootDir = new DirectoryInfo("/");

                        foreach (DirectoryInfo dir in rootDir.EnumerateDirectories())
                        {

                            string fullPath = dir.FullName.ToLowerInvariant();

                            // se è una cartella di sistema viene esclusa
                            if (Array.Exists(_excludedDirectories, ex => fullPath.StartsWith(ex)))
                            {
                                
                                //Console.WriteLine($"Ignored system directory: [yellow]{dir.Name}[/]");
                                continue;
                            }

                            ctx.Status($"Reading subdirectory: [green]{dir.Name}[/]");

                            var drive = new DriveInfo(dir.FullName);

                            // ! legge circa 7GB di troppo
                            // ! da risolvere
                            ScanDirectory(dir, ref _totalSizeInBytes, ref _totalFilesVisited, ctx);
                        }


                        AnsiConsole.Markup("[green]Operation completed[/]\n");
                    }
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            _diskAttributes.TotalSizeOccupiedInBytes = _totalSizeInBytes;
            _diskAttributes.TotalFilesVisited = _totalFilesVisited;

            return _diskAttributes;
        }


        // si passano i riferimenti (ref) alle variabili che si volgiono modificare
        private static void ScanDirectory(DirectoryInfo dir, ref long totalBytes, ref long totalFiles, StatusContext ctx)
        {
            // opzioni per l'enumerazione dei file
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true,
                AttributesToSkip = FileAttributes.ReparsePoint // Ignora symlinks
            };

            try
            {
                // ottiene l'enumeratore nel quale ci si può muovere manualmente con MoveNext()
                using var enumerator = dir.EnumerateFiles("*", options).GetEnumerator();

                while (true)
                {
                    try
                    {
                        // se l'enumeratore non riesce a muoversi al prossimo elemento
                        if (!enumerator.MoveNext())
                            break;

                        FileInfo file = enumerator.Current;
                        totalBytes += file.Length;
                        totalFiles++;
                    }
                    catch { /**/ }
                }
            }
            catch { /**/ }
        }

        /*
            TODO LETTURA SPAZIO MASSIMO DEL DISCO
        */

    }
}