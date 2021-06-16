using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Net.Client;
using Intra_Block.Cache;
using Intra_Block.COM;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Intra_Block.Tests.Client
{
    [TestFixture]
    public class When_Setting
    {
        private Intra.IntraClient IntraClient;
        private HttpClient Client;

        [SetUp]
        public void SetUp()
        {
            var factory = new WebApplicationFactory<Startup>();
            Client = factory.CreateClient();

            var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {
                HttpClient = Client
            });

            IntraClient = new Intra.IntraClient(channel);
        }

        [Test]
        public async Task Should_Set()
        {
            var result = IntraClient.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            });

            result.Successful.Should().BeTrue();
        }

        [Test]
        public async Task Should_Exceed_Size()
        {
            IntraClient.Insert(new InsertionRequest()
            {
                Key = "World",
                Value = "Hello",
                Expiry = 0
            });
            
            IntraClient.Insert(new InsertionRequest()
            {
                Key = "World2",
                Value = "Hello",
                Expiry = 0
            });
            
            IntraClient.Insert(new InsertionRequest()
            {
                Key = "World3",
                Value = "Hello",
                Expiry = 0
            });
            
            IntraClient.Insert(new InsertionRequest()
            {
                Key = "World4",
                Value = "Hello",
                Expiry = 0
            });
            
            var result = IntraClient.Insert(new InsertionRequest()
            {
                Key = "Hello",
                Value = "World",
                Expiry = 0
            });

            result.Successful.Should().BeFalse();
        }
    }
}