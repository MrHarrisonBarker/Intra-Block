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
    public class When_Retrieving
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
        public async Task Should_Retrieve()
        {
            IntraClient.Insert(new InsertionRequest() { Key = "Hello", Value = "World" });
            
            var result = IntraClient.Retrieve(new RetrievalRequest()
            {
                Key = "Hello"
            });

            result.Value.Should().BeEquivalentTo("World");
        }

        [Test]
        public async Task Should_Not_Find()
        {
            FluentActions.Invoking(() => IntraClient.Retrieve(new RetrievalRequest()
            {
                Key = "DOES NOT EXIST"
            })).Should().Throw<Grpc.Core.RpcException>();
        }
    }
}