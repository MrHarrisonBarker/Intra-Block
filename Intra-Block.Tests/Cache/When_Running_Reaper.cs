using System;
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
        private ServiceCollection Services;
        private Intra_Block.Cache.Cache Cache;

        [SetUp]
        public void SetUp()
        {
            Services = new ServiceCollection();
            var serviceProvider = Services
                .AddSingleton<Intra_Block.Cache.Cache>()
                .AddSingleton<Administratum>()
                // .AddHostedService<GrimReaper>(provider => new GrimReaper(provider.GetService<ILogger<GrimReaper>>(), provider.GetService<Intra_Block.Cache.Cache>(),
                //     provider.GetService<Administratum>(), 100))
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Administratum = serviceProvider.GetService<Administratum>();
            Cache = serviceProvider.GetService<Intra_Block.Cache.Cache>();

            // Administratum = new Administratum(new Logger<Administratum>(LoggerFactory));
        }

        [Test]
        public async Task Should_Reap_Old_Key()
        {
            Cache.NumberOfEntries().Should().Be(0);
            
            Cache.Insert("key", "Hello World", 10);
            
            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), Cache, Administratum);

            await Task.Delay(20);
            
            reaper.CheckOnCache(null);

            Cache.NumberOfEntries().Should().Be(0);
        }

        [Test]
        public async Task Should_Not_Reap_Fresh_Key()
        {
            Cache.NumberOfEntries().Should().Be(0);
            
            Cache.Insert("key", "Hello World", 1000);

            var reaper = new GrimReaper(new Logger<GrimReaper>(LoggerFactory), Cache, Administratum);

            await Task.Delay(20);
            
            reaper.CheckOnCache(null);

            Cache.NumberOfEntries().Should().Be(1);
        }
    }
}