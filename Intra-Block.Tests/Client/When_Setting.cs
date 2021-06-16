using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using Intra_Block.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Intra_Block.Tests.Client
{
    [TestFixture]
    public class When_Setting
    {
        private ILoggerFactory LoggerFactory;
        private IntraCache IntraCache;
        private Mock<Intra.IntraClient> MockClient;

        [SetUp]
        public void SetUp()
        {
            // var serviceProvider = new ServiceCollection()
            // serviceCollection.AddGrpcClient<new Mock<Intra.IntraClient>().Object>(options => { options.Address = new Uri(""); });
            // serviceCollection.AddTransient<IntraCache>();

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            MockClient = new Mock<Intra.IntraClient>();

            IntraCache = new IntraCache(new Logger<IntraCache>(LoggerFactory), MockClient.Object);
        }

        [Test]
        public async Task Should_Set()
        {
            MockClient.Setup(client => client.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            }, null, null, default(global::System.Threading.CancellationToken)));

            // MockClient.Setup(c => c.Insert())

            IntraCache.Set("Hello", Encoding.UTF8.GetBytes("World"), null);
        }

        [Test]
        public async Task Should_Exceed_Size()
        {
            throw new NotImplementedException();
        }
    }
}