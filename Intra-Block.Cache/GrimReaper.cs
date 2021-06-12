using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Intra_Block.Cache
{
    public interface IGrimReaper
    {
        void CheckOnCache();
    }
    
    public class GrimReaper : IHostedService, IDisposable, IGrimReaper
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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