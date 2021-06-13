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
        
        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }
        
        [Test]
        public async Task Should_Reap_Old_Key()
        {
            var cache = new Intra_Block.Cache.Cache();
            cache.Insert("key", "Hello World", 1);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache);

            var ct = new CancellationTokenSource();
            ct.CancelAfter(1000);
            await reaper.StartAsync(ct.Token);

            cache.NumberOfEntries().Should().Be(0);
        }

        [Test]
        public async Task Should_Not_Reap_Fresh_Key()
        {
            var cache = new Intra_Block.Cache.Cache();
            cache.Insert("key", "Hello World", 100);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), cache);

            var ct = new CancellationTokenSource();
            ct.CancelAfter(1000);
            await reaper.StartAsync(ct.Token);

            cache.NumberOfEntries().Should().Be(1);
        }
    }
}