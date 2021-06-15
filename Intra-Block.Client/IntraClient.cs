using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Intra_Block.COM;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Intra_Block.Client
{
    public class IntraCache : IDistributedCache
    {
        private readonly ILogger<IntraCache> Logger;
        private readonly Intra.IntraClient Client;

        public IntraCache(ILogger<IntraCache> logger, Intra.IntraClient intraClient)
        {
            Logger = logger;
            Client = intraClient;
        }

        public byte[] Get(string key)
        {
            try
            {
                // TODO: convert to using byte arrays in data transmission
                return Encoding.UTF8.GetBytes(Client.Retrieve(new RetrievalRequest()
                {
                    Key = key
                }).Value);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            try
            {
                Client.Insert(new InsertionRequest()
                {
                    Key = key,
                    Value = value.ToString(),
                    Expiry = options.AbsoluteExpiration.HasValue ? (ulong)(options.AbsoluteExpiration.Value.Ticks / TimeSpan.TicksPerMillisecond) : 0
                });
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            try
            {
                Client.Remove(new RemoveRequest()
                {
                    Key = key
                });
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        public Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}