using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Reporting_Insertion
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
        public async Task Should_Calculate_Average()
        {
            Administratum.ReportInsertion(100);

            Administratum.GatherReport().Averages.TimeToInsert.Should().Be(0.1);
            
            Administratum.ReportInsertion(120);
            
            Administratum.GatherReport().Averages.TimeToInsert.Should().Be(0.11);
        }
    }
}