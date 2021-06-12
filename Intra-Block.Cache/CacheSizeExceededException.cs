using System;

namespace Intra_Block.Cache
{
    public class CacheSizeExceededException : Exception
    {
        public CacheSizeExceededException() : base("The maximum size of the cache has been exceeded")
        {
        }
    }
}