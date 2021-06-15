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
    public partial class When_Inserting
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
        }

        [Test]
        public async Task Should_Insert()
        {
            Cache.Keys().Count.Should().Be(0);

            var result = await IntraService.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            }, new MockServerContext());

            result.Should().BeEquivalentTo(new GenericResponse()
            {
                Successful = true
            });

            Cache.Keys().Count.Should().Be(1);
        }

        [Test]
        public async Task Should_Update_Existing_Key()
        {
            Cache.Keys().Count.Should().Be(0);

            await IntraService.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            }, new MockServerContext());

            var result = await IntraService.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "Hello World",
                Expiry = 0
            }, new MockServerContext());

            result.Should().BeEquivalentTo(new GenericResponse()
            {
                Successful = true
            });

            Cache.Keys().Count.Should().Be(1);
            Cache.Retrieve("Hello").Should().BeEquivalentTo("Hello World");
        }

        [Test]
        public async Task Should_Error_when_Size_Exceeded()
        {
            Cache.Keys().Count.Should().Be(0);

            await IntraService.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            }, new MockServerContext());

            var result = await IntraService.Insert(new InsertionRequest()
            {
                Key = "Another",
                Value = "Hello World",
                Expiry = 0
            }, new MockServerContext());

            result.Should().BeEquivalentTo(new GenericResponse()
            {
                Successful = false,
                Message = "The maximum size of the cache has been exceeded trying to fit 42 into 30"
            });

            Cache.Keys().Count.Should().Be(1);
        }
    }
}