namespace DiskUsageAnalyzer.Models
{
    public class DiskAttributes(long bytesOccupied, long bytesTotal, long filesVisited)
    {
        public long TotalSizeOccupiedInBytes { get; set; } = bytesOccupied;
        public long TotalDiskSizeInBytes { get; set; } = bytesTotal;
        public long TotalFilesVisited { get; set; } = filesVisited;

        public DiskAttributes() : this(0, 0, 0)
        {  
        }
    }
}