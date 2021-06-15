using System;

namespace Intra_Block.Cache
{
    public static class Utils
    {
        public static long TimeInMicroSeconds()
        {
            return DateTime.Now.Ticks / 10;
        }
    }
}