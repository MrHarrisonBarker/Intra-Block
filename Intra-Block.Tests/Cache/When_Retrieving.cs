using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Retrieving
    {
        private ILoggerFactory LoggerFactory;
        private Intra_Block.Cache.Cache Cache;
        private Administratum Administratum;
        
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
        public async Task Should_Retrieve()
        {
            Cache.Retrieve("key").Should().BeEquivalentTo("Hello World");
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(() => Cache.Exterminatus("SomeRandomKey")).Should().Throw<DoesNotExistException>();
        }
    }
}