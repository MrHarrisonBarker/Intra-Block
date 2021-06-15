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
    public class When_Reporting
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
        public async Task Should_Report()
        {
            var result = await IntraService.Report(new ReportRequest(), new When_Inserting.MockServerContext());
            result.Should().NotBeNull();
        }
    }
}