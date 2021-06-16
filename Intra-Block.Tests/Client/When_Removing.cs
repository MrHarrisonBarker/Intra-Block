using System;
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
    public class When_Removing
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
        public async Task Should_Remove()
        {
            IntraClient.Insert(new InsertionRequest() { Key = "Hello", Value = "World" });
            
            var result = IntraClient.Remove(new RemoveRequest()
            {
                Key = "Hello"
            });

            result.Successful.Should().BeTrue();
        }

        [Test]
        public async Task Should_Not_Find()
        {
            var result = IntraClient.Remove(new RemoveRequest()
            {
                Key = "DOES NOT EXIST"
            });

            result.Successful.Should().BeFalse();
            result.Message.Should().BeEquivalentTo("\"DOES NOT EXIST\" doesn't exist in the cache");
        }
    }
}