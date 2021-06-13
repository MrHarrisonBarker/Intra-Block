using System;
using System.Collections;
using System.Collections.Generic;

namespace Intra_Block.Cache
{
    public interface ICache
    {
        string Retrieve(string key);
        void Insert(string key, string value, int expiry);
        void Exterminatus(string key);
        int NumberOfEntries();
        ICollection<CacheEntry> Entries();
        ICollection<string> Keys();
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

        public void Insert(string key, string value, int expiry = 0)
        {
            if (CacheStore.ContainsKey(key))
            {
                CacheStore[key].Data = value;
            }
            else
            {
                var newValueSize = SizeOfEntry(value);

                if (CacheSize + newValueSize > MaximumSize) throw new CacheSizeExceededException(newValueSize,CacheSize);

                CacheStore.Add(key, new CacheEntry()
                {
                    Data = value,
                    Updated = DateTime.Now,
                    LastRetrieval = DateTime.Now,
                    Expiry = expiry
                });

                CacheSize += newValueSize;
            }
        }

        private static int SizeOfEntry(string value)
        {
            return sizeof(char) * value.Length + 16 + 4;
        }

        public void Exterminatus(string key)
        {
            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);

            CacheSize -= SizeOfEntry(CacheStore[key].Data);
            
            CacheStore.Remove(key);
        }

        public int NumberOfEntries()
        {
            return CacheStore.Values.Count;
        }

        public ICollection<CacheEntry> Entries()
        {
            return CacheStore.Values;
        }

        public ICollection<string> Keys()
        {
            return CacheStore.Keys;
        }
    }
}