using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Inserting
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
            
            Cache = new Intra_Block.Cache.Cache(Administratum, new Logger<Intra_Block.Cache.Cache>(LoggerFactory),42);
        }

        [Test]
        public async Task Should_Insert()
        {
            Cache.Insert("key", "Hello World");
            Cache.NumberOfEntries().Should().Be(1);
        }

        [Test]
        public async Task Should_Exceed_Cache_Size()
        {
            Cache.Insert("key", "Hello World");
            FluentActions.Invoking(() => Cache.Insert("AnotherKey", "Hello World")).Should().Throw<CacheSizeExceededException>();
        }

        [Test]
        public async Task Should_Insert_Existing_Key()
        {
            Cache.Insert("key", "Hello World");
            var numOfEntries = Cache.NumberOfEntries();
            Cache.Insert("key", "World Hello");
            numOfEntries.Should().Be(Cache.NumberOfEntries());
            Cache.Retrieve("key").Should().BeEquivalentTo("World Hello");
        }
    }
}