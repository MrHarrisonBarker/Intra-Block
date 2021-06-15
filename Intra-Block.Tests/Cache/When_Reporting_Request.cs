using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Reporting_Request
    {
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
        }

        [Test]
        public async Task Should_Calculate_Average_Speed()
        {
            Administratum.ReportRequest(1241);
            Administratum.ReportRequest(2351);
            Administratum.ReportRequest(3266);
            Administratum.ReportRequest(3452);
            Administratum.ReportRequest(5425);

            Administratum.GatherReport().Averages.RequestCompletion.Should().Be(4.20825);
        }

        [Test]
        public async Task Should_Calculate_Per_Min()
        {
            Administratum.ReportRequest(100);
            Administratum.ReportRequest(100);
            Administratum.ReportRequest(100);
            
            // TODO : is this truly a good representation of the speed?
            Administratum.GatherReport().Averages.RequestsPerMinute.Should().Be(1.5);
        }
    }
}