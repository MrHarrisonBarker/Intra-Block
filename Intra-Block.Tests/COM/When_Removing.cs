using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Intra_Block.COM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.COM
{
    [TestFixture]
    public class When_Removing
    {
        private Intra_Block.Cache.Cache Cache;
        private Administratum Administratum;
        private ILoggerFactory LoggerFactory;
        private IntraService IntraService;

        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Administratum = new Administratum(new Logger<Administratum>(LoggerFactory));

            Cache = new Intra_Block.Cache.Cache(Administratum, new Logger<Intra_Block.Cache.Cache>(LoggerFactory), 42);

            IntraService = new IntraService(new Logger<IntraService>(LoggerFactory), Cache, Administratum);

            Cache.Insert("Hello", "World");
        }

        [Test]
        public async Task Should_Remove()
        {
            var result = await IntraService.Remove(new RemoveRequest()
            {
                Key = "Hello"
            }, new When_Inserting.MockServerContext());
            
            result.Should().BeEquivalentTo(new GenericResponse()
            {
                Successful = true
            });

            Cache.NumberOfEntries().Should().Be(0);
        }

        [Test]
        public async Task Should_Not_Find()
        {
            var result = await IntraService.Remove(new RemoveRequest()
            {
                Key = "World"
            }, new When_Inserting.MockServerContext());
            
            result.Should().BeEquivalentTo(new GenericResponse()
            {
                Successful = false,
                Message = "\"World\" doesn't exist in the cache"
            });

            Cache.NumberOfEntries().Should().Be(1);
        }
    }
}