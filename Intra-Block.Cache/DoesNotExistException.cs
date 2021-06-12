using System;

namespace Intra_Block.Cache
{
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException(string key) : base($"\"{key}\" doesn't exist in the cache") {}
    }
}