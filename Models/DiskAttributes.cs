using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiskUsageAnalyzer.Models
{
    public class DiskAttributes
    {
        public long TotalSizeOccupiedInBytes { get; set; }
        public long TotalDiskSizeInBytes { get; set; }
        public long TotalFilesVisited { get; set; }

        public DiskAttributes(long bytesOccupied, long bytesTotal, long filesVisited)
        {
            TotalSizeOccupiedInBytes = bytesOccupied;
            TotalDiskSizeInBytes = bytesTotal;
            TotalFilesVisited = filesVisited;
        }

        public DiskAttributes()
        {
            TotalSizeOccupiedInBytes = 0;
            TotalDiskSizeInBytes = 0;
            TotalFilesVisited = 0;
        }
    }
}