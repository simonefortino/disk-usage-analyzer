package Tools;

public record DiskAttributes(long filesVisited, long totalSize, long filesNotVisited) {
    public void printAttributes () {
        System.out.println("File visitati: " + filesVisited);
        System.out.println("File non visitati: " + filesNotVisited);
        System.out.println("Grandezza totale files in bytes: " + totalSize);
    }
}
