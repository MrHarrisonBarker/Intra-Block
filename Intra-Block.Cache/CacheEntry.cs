using System;

namespace Intra_Block.Cache
{
    public class CacheEntry
    {
        public string Data { get; set; }
        public DateTime Updated { get; set; }
        public DateTime LastRetrieval { get; set; }
        public ulong Expiry { get; set; }
    }
}