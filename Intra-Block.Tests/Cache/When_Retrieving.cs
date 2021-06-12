using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Retrieving
    {
        private Intra_Block.Cache.Cache Cache;
        
        [SetUp]
        public void SetUp()
        {
            Cache = new Intra_Block.Cache.Cache();
            Cache.Insert("key","Hello World");
        }

        [Test]
        public async Task Should_Retrieve()
        {
            Cache.Retrieve("key").Should().BeEquivalentTo("Hello World");
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(() => Cache.Exterminatus("SomeRandomKey")).Should().Throw<DoesNotExistException>();
        }
    }
}