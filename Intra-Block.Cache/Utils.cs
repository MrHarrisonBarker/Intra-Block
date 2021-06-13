using System;

namespace Intra_Block.Cache
{
    public static class Utils
    {
        public static long TimeInMilliseconds()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}