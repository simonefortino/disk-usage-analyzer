using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiskUsageAnalyzer.Models;
using Spectre.Console;


namespace DiskUsageAnalyzer.Engine
{
    public static class DiskAnalyzer
    {
        private static long totalSizeInBytes = 0;
        private static long totalFilesVisited = 0;

        private static DiskAttributes diskAttributes = new DiskAttributes();

        // cartelle di sistema da escludere
        private static string[] excludedDirectories = 
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

                AnsiConsole.Status().Start(
                    "Analyzing subdirecotries...",
                    ctx =>
                    {

                        DirectoryInfo rootDir = new DirectoryInfo("/");

                        foreach (DirectoryInfo dir in rootDir.EnumerateDirectories())
                        {

                            string fullPath = dir.FullName.ToLowerInvariant();

                            // se è una cartella di sistema viene esclusa
                            if (Array.Exists(excludedDirectories, ex => fullPath.StartsWith(ex)))
                            {
                                
                                //Console.WriteLine($"Ignored system directory: [yellow]{dir.Name}[/]");
                                continue;
                            }

                            ctx.Status($"Reading directory: [green]{dir.Name}[/]");

                            var drive = new DriveInfo(dir.FullName);

                            // ! legge circa 7GB di troppo
                            // ! da risolvere
                            ScanDirectory(dir, ref totalSizeInBytes, ref totalFilesVisited, ctx);
                        }


                        AnsiConsole.Markup("[green]Operation completed[/]\n");
                    }
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            diskAttributes.TotalSizeOccupiedInBytes = totalSizeInBytes;
            diskAttributes.TotalFilesVisited = totalFilesVisited;

            return diskAttributes;
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
                        // se l'enumaeratore non riesce a muoversi al prossimo elemento
                        if (!enumerator.MoveNext())
                            break;

                        FileInfo file = enumerator.Current;
                        totalBytes += file.Length;
                        totalFiles++;
                    }
                    catch
                    {
                        // Cattura eccezioni sui singoli file (es. file cancellati al volo)
                    }
                }
            }
            catch
            {
                // Cattura eccezioni se l'intera cartella fallisce l'apertura dell'enumeratore
            }
        }

        /*
            TODO LETTURA SPAZIO MASSIMO DEL DISCO
        */

    }
}