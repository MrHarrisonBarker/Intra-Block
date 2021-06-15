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
        void CheckOnCache(object state);
    }

    public class GrimReaper : IHostedService, IDisposable, IGrimReaper
    {
        private readonly ILogger<GrimReaper> Logger;
        private readonly Cache Cache;
        private readonly Administratum Administratum;
        private Timer Timer;

        public GrimReaper(ILogger<GrimReaper> logger, Cache cache, Administratum administratum)
        {
            Logger = logger;
            Cache = cache;
            Administratum = administratum;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Timer = new Timer(CheckOnCache, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void Reap(string key)
        {
            try
            {
                Cache.Exterminatus(key);
                Administratum.ReportReap();
            }
            catch (DoesNotExistException)
            {
                Logger.LogInformation("The reaper is trying to exterminatus an entry that doesn't exist.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("The Grim Reaper has met an ill fate");

            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void CheckOnCache(object state)
        {
            Logger.LogInformation("Checking on cache");

            for (int i = 0; i < Cache.Entries().Count; i++)
            {
                var entry = Cache.Entries().ToArray()[i];

                if (entry.Expiry == 0) continue;

                var timeFromLast = DateTime.Now - entry.LastRetrieval;

                // if the data is now stale get rid of it
                if (timeFromLast.TotalMilliseconds > entry.Expiry)
                {
                    Logger.LogInformation("Found stale data");

                    Reap(Cache.Keys().ToArray()[i]);
                }
            }
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}