namespace Intra_Block.Cache
{
    public class Report
    {
        public ulong NumberOfEntries { get; set; }
        public ulong CurrentMemoryUsage { get; set; }
        public ulong Uptime { get; set; }
        public ConnectedClient Clients { get; set; }
        public Averages Averages { get; set; }
        
    }
}