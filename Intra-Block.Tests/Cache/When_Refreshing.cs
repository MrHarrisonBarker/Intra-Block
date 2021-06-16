using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Refreshing
    {
        private Intra_Block.Cache.Cache Cache;
        private Administratum Administratum;
        private ILoggerFactory LoggerFactory;

        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Administratum = new Administratum(new Logger<Administratum>(LoggerFactory));

            Cache = new Intra_Block.Cache.Cache(Administratum, new Logger<Intra_Block.Cache.Cache>(LoggerFactory), 42);
        }

        [Test]
        public async Task Should_Refresh()
        {
            Cache.Insert("Hello", "World");
            Cache.Refresh("Hello");
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(() => Cache.Refresh("DOES NOT EXIST")).Should().Throw<DoesNotExistException>();
        }
    }
}