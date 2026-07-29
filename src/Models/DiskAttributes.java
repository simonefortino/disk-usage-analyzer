package Models;

public record DiskAttributes(long filesVisited, long filesNotVisited, long usedSpace, long totalSpace ) {
    public void printAttributes () {
        System.out.println("File visitati: " + filesVisited);
        System.out.println("File non visitati: " + filesNotVisited);
        System.out.println("Dimensione totale files: " + toMegabytes(usedSpace) + " MB");
        System.out.println("Spazio totale sulla partizione: " + toMegabytes(totalSpace) + " MB");
    }

    private long toMegabytes(long input) {
        return input / 1000000;
    }
}
