using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Inserting
    {
        private Intra_Block.Cache.Cache Cache;

        [SetUp]
        public void SetUp()
        {
            Cache = new Intra_Block.Cache.Cache(38);
        }

        [Test]
        public async Task Should_Insert()
        {
            Cache.Insert("key", "Hello World");
            Cache.NumberOfEntries().Should().Be(1);
        }

        [Test]
        public async Task Should_Exceed_Cache_Size()
        {
            Cache.Insert("key", "Hello World");
            FluentActions.Invoking(() => Cache.Insert("AnotherKey", "Hello World")).Should().Throw<CacheSizeExceededException>();
        }

        [Test]
        public async Task Should_Insert_Existing_Key()
        {
            Cache.Insert("key", "Hello World");
            var numOfEntries = Cache.NumberOfEntries();
            Cache.Insert("key", "World Hello");
            numOfEntries.Should().Be(Cache.NumberOfEntries());
            Cache.Retrieve("key").Should().BeEquivalentTo("World Hello");
        }
    }
}