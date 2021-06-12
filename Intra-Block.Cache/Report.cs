namespace Intra_Block.Cache
{
    public class Report
    {
        public int NumberOfEntries { get; set; }
        public int CurrentMemoryUsage { get; set; }
        public int Uptime { get; set; }
        public ConnectedClient Clients { get; set; }
        public Averages Averages { get; set; }
        
    }
}