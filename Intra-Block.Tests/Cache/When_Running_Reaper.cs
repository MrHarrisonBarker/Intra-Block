using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Running_Reaper
    {
        private ILoggerFactory LoggerFactory;
        private Administratum Administratum;

        [SetUp]
        public void SetUp()
        {
            Administratum = new Administratum();

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }

        [Test]
        public async Task Should_Reap_Old_Key()
        {
            var cache = new Intra_Block.Cache.Cache(Administratum);
            cache.Insert("key", "Hello World", 10);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache, Administratum);

            var ct = new CancellationTokenSource();
            ct.CancelAfter(100);
            await reaper.StartAsync(ct.Token);

            cache.NumberOfEntries().Should().Be(0);
        }

        [Test]
        public async Task Should_Not_Reap_Fresh_Key()
        {
            var cache = new Intra_Block.Cache.Cache(Administratum);
            cache.Insert("key", "Hello World", 1000);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache, Administratum);

            var ct = new CancellationTokenSource();
            ct.CancelAfter(100);
            await reaper.StartAsync(ct.Token);

            cache.NumberOfEntries().Should().Be(1);
        }
    }
}