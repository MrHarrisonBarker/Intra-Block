using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Intra_Block.Cache
{
    public interface ICache
    {
        string Retrieve(string key);
        void Insert(string key, string value, ulong expiry);
        void Exterminatus(string key);
        void Refresh(string key);
        int NumberOfEntries();
        ICollection<CacheEntry> Entries();
        ICollection<string> Keys();
    }

    public class Cache : ICache
    {
        private readonly IDictionary<string, CacheEntry> CacheStore = new Dictionary<string, CacheEntry>();
        private int CacheSize;
        private readonly int MaximumSize;
        private readonly Administratum Administratum;
        private readonly ILogger<Cache> Logger;

        public Cache(Administratum administratum, ILogger<Cache> logger, int maximumSize = 100)
        {
            MaximumSize = maximumSize;
            Administratum = administratum;
            Logger = logger;
        }

        public string Retrieve(string key)
        {
            var s = Utils.TimeInMicroSeconds();

            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);

            CacheStore[key].LastRetrieval = DateTime.Now;

            Administratum.ReportRetrieval(Math.Abs(Utils.TimeInMicroSeconds() - s));

            return CacheStore[key].Data;
        }

        public void Insert(string key, string value, ulong expiry = 0)
        {
            var s = Utils.TimeInMicroSeconds();

            if (CacheStore.ContainsKey(key))
            {
                CacheStore[key].Data = value;
            }
            else
            {
                var newValueSize = SizeOfEntry(value);

                if (CacheSize + newValueSize > MaximumSize) throw new CacheSizeExceededException(newValueSize, CacheSize);

                CacheStore.Add(key, new CacheEntry
                {
                    Data = value,
                    Updated = DateTime.Now,
                    LastRetrieval = DateTime.Now,
                    Expiry = expiry
                });

                CacheSize += newValueSize;
            }

            Administratum.ReportInsertion(Math.Abs(Utils.TimeInMicroSeconds() - s));
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

        public void Refresh(string key)
        {
            if (!CacheStore.ContainsKey(key)) throw new DoesNotExistException(key);
            
            CacheStore[key].LastRetrieval = DateTime.Now;
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