using System;
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
    public class When_Retrieving
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
            
            Cache.Insert("Hello","World");
        }

        [Test]
        public async Task Should_Retrieve()
        {
            var result = await IntraService.Retrieve(new RetrievalRequest()
            {
                Key = "Hello"
            }, new When_Inserting.MockServerContext());

            result.Value.Should().BeEquivalentTo("World");
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(async () => await IntraService.Retrieve(new RetrievalRequest()
            {
                Key = "World"
            }, new When_Inserting.MockServerContext())).Should().Throw<DoesNotExistException>();
        }
    }
}