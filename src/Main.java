import java.nio.file.Path;
import java.nio.file.Paths;

import Tools.DiskAttributes;
import Tools.FileAnalyzer;

/**
 * Main
 */
public class Main {
    public static void main(String[] args) {

        DiskAttributes analisysResult;
        FileAnalyzer analyzer = new FileAnalyzer();
        Path path = Paths.get("/");

        analisysResult = analyzer.Analyze(path);

        analisysResult.printAttributes();

        
    }
}