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

    public class GrimReaper : BackgroundService
    {
        private readonly ILogger<GrimReaper> Logger;
        private readonly Cache Cache;
        private readonly Administratum Administratum;
        private Timer Timer;
        private readonly int CheckInterval;

        public GrimReaper(ILogger<GrimReaper> logger, Cache cache, Administratum administratum, int checkInterval = 60000)
        {
            Logger = logger;
            Cache = cache;
            Administratum = administratum;
            CheckInterval = checkInterval;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                CheckOnCache(null);
                await Task.Delay(CheckInterval, cancellationToken);
            }
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
    }
}