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
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
            
            Administratum = new Administratum(new Logger<Administratum>(LoggerFactory));
        }

        [Test]
        public async Task Should_Reap_Old_Key()
        {
            var cache = new Intra_Block.Cache.Cache(Administratum,new Logger<Intra_Block.Cache.Cache>(LoggerFactory));
            cache.Insert("key", "Hello World", 1);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache, Administratum);
            await reaper.StartAsync(new CancellationToken());
            
            await Task.Delay(10);

            cache.NumberOfEntries().Should().Be(0);
        }

        [Test]
        public async Task Should_Not_Reap_Fresh_Key()
        {
            var cache = new Intra_Block.Cache.Cache(Administratum,new Logger<Intra_Block.Cache.Cache>(LoggerFactory));
            cache.Insert("key", "Hello World", 1000);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache, Administratum);

            await reaper.StartAsync(new CancellationToken());
            
            await Task.Delay(100);

            cache.NumberOfEntries().Should().Be(1);
        }
    }
}