using System;

namespace Intra_Block.Cache
{
    public class CacheSizeExceededException : Exception
    {
        public CacheSizeExceededException(int tryingToFit, int into) : base($"The maximum size of the cache has been exceeded trying to fit {tryingToFit} into {into}")
        {
        }
    }
}