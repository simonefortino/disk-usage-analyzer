package Engine;
import java.io.IOException;
import java.nio.file.FileVisitResult;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.SimpleFileVisitor;
import java.nio.file.attribute.BasicFileAttributes;
import java.util.Scanner;

public class FileAnalyzer {
    private long filesVisited;
    private long totalSize;
    private long filesNotVIsited;

    public FileAnalyzer() {
        filesVisited = 0;
        filesNotVIsited = 0;
        totalSize = 0;
    }

    public DiskAttributes Analyze(Path path) {

        Scanner scanner = new Scanner(System.in);
        //System.out.print("Type the path: ");
        //String inputPath = scanner.nextLine();

        System.out.println("Cartella in analisi: " + path.toAbsolutePath());

        try {
            Files.walkFileTree(path, new SimpleFileVisitor<Path>() {
                // metodo invocato prima di entrare in una cartella
                @Override
                public FileVisitResult preVisitDirectory(Path dir, BasicFileAttributes attrs) {
                    String dirName = dir.toString();
                    
                    // ignora le cartelle virtuali
                    if (dirName.equals("/proc") || 
                        dirName.equals("/sys")  || 
                        dirName.equals("/dev")  || 
                        dirName.equals("/run")) {
                        
                        return FileVisitResult.SKIP_SUBTREE; // ignora la cartella
                    }
                    return FileVisitResult.CONTINUE;
                }


                // metodo che viene chiamato a ogni visita di un file
                @Override
                public FileVisitResult visitFile(Path file, BasicFileAttributes attrs)
                    throws IOException
                {

                    if(attrs.isRegularFile()) {
                        filesVisited++;
                        totalSize += attrs.size();

                        // stampa dimensione e path di ogni file
                        System.out.println(attrs.size() + "\t" + file.toString());
                    }
                    

                    return FileVisitResult.CONTINUE;
                }

                // metodo invocato quando fallisce la lettura di un file
                @Override
                public FileVisitResult visitFileFailed(Path file, IOException exc) {
                    // se la lettura fallisce ignoriamo il file, questo risulterà in un conteggio più basso 
                    // rispetto a quello reale

                    //System.out.println(exc.getMessage());
                    filesNotVIsited++;
                    return FileVisitResult.CONTINUE;
                }
            });
        } catch (Exception e) {
            System.out.println(e.getMessage());
            e.printStackTrace();
        }

        scanner.close();

        return new DiskAttributes(filesVisited, totalSize, filesNotVIsited);
    }
}
