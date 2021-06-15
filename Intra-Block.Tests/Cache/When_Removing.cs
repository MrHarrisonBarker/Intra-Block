using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Removing
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
            
            Cache = new Intra_Block.Cache.Cache(Administratum,new Logger<Intra_Block.Cache.Cache>(LoggerFactory));
            Cache.Insert("key","Hello World");
        }

        [Test]
        public async Task Should_Remove()
        {
            var initSize = Cache.NumberOfEntries();
            Cache.Exterminatus("key");
            Cache.NumberOfEntries().Should().BeLessThan(initSize);
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(() => Cache.Exterminatus("SomeRandomKey")).Should().Throw<DoesNotExistException>();
        }
    }
}