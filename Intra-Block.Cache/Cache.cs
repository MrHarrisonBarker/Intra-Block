using System;
using System.Collections.Generic;

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
        private readonly IDictionary<string, CacheEntry> CacheStore = new Dictionary<string, CacheEntry>();
        private int CacheSize;
        private readonly int MaximumSize;

        public Cache(int maximumSize = 100)
        {
            MaximumSize = maximumSize;
        }

        public string Retrieve(string key)
        {
            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);

            CacheStore[key].LastRetrieval = DateTime.Now;

            return CacheStore[key].Data;
        }

        public void Insert(string key, string value)
        {
            if (CacheStore.ContainsKey(key))
            {
                CacheStore[key].Data = value;
            }
            else
            {
                var newValueSize = sizeof(char) * value.Length + 16;

                if (CacheSize + newValueSize > MaximumSize) throw new CacheSizeExceededException();

                CacheStore.Add(key, new CacheEntry()
                {
                    Data = value,
                    Updated = DateTime.Now,
                    LastRetrieval = DateTime.Now
                });

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