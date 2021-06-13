using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Intra_Block.Cache
{
    public interface IGrimReaper
    {
        void CheckOnCache();
    }
    
    public class GrimReaper : IHostedService, IDisposable, IGrimReaper
    {
        private readonly ILogger<GrimReaper> Logger;
        private readonly Cache Cache;

        public GrimReaper(ILogger<GrimReaper> logger, Cache cache)
        {
            Logger = logger;
            Cache = cache;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < Cache.Entries().Count; i++)
                {
                    var entry = Cache.Entries().ToArray()[i];
                    
                    if (entry.Expiry == 0) continue;

                    var timeFromLast = DateTime.Now - entry.LastRetrieval;

                    // if the data is now stale get rid of it
                    if (timeFromLast.TotalSeconds > entry.Expiry)
                    {
                        Reap(Cache.Keys().ToArray()[i]);
                    }
                }
            }


            return Task.CompletedTask;
        }

        private void Reap(string key)
        {
            try
            {
                Cache.Exterminatus(key);
                // TODO: Administratum call for average reaping
            }
            catch (DoesNotExistException doesNotExistException)
            {
                Logger.LogInformation("The reaper is trying to exterminatus an entry that doesn't exist.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void CheckOnCache()
        {
            throw new NotImplementedException();
        }
    }
}