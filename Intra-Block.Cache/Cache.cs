using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Intra_Block.Cache
{
    public interface ICache
    {
        string Retrieve(string key);
        void Insert(string key, string value);
        void Exterminatus(string key);
        int NumberOfEntries();
    }

    public class Cache : ICache
    {
        private readonly IDictionary<string, string> CacheStore = new Dictionary<string, string>();
        private int CacheSize;
        private readonly int MaximumSize;

        public Cache(int maximumSize = 100)
        {
            MaximumSize = maximumSize;
        }

        public string Retrieve(string key)
        {
            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);

            return CacheStore[key];
        }

        public void Insert(string key, string value)
        {
            if (CacheStore.ContainsKey(key))
            {
                CacheStore[key] = value;
            }
            else
            {
                var newValueSize = sizeof(char) * value.Length;
                
                if (CacheSize + newValueSize > MaximumSize) throw new CacheSizeExceededException();

                CacheStore[key] = value;

                CacheSize += newValueSize;
            }
        }

        public void Exterminatus(string key)
        {
            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);

            CacheStore.Remove(key);
        }

        public int NumberOfEntries()
        {
            return CacheStore.Values.Count;
        }
    }
}