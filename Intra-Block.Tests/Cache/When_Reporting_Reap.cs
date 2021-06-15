using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Reporting_Reap
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
        public async Task Should_Calculate_Per_Min()
        {
            Administratum.ReportReap();
            Administratum.ReportReap();
            Administratum.ReportReap();

            // TODO : is this truly a good representation of the speed?
            Administratum.GatherReport().Averages.ReapsPerMinute.Should().Be(2);
        }
    }
}