using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Removing
    {
        private Intra_Block.Cache.Cache Cache;
        
        [SetUp]
        public void SetUp()
        {
            Cache = new Intra_Block.Cache.Cache();
            Cache.Insert("key","Hello World");
        }

        [Test]
        public async Task Should_Remove()
        {
            var initSize = Cache.NumberOfEntries();
            Cache.Exterminatus("key");
            Cache.NumberOfEntries().Should().BeLessThan(initSize);
        }

        [Test]
        public async Task Should_Not_Find_Key()
        {
            FluentActions.Invoking(() => Cache.Exterminatus("SomeRandomKey")).Should().Throw<DoesNotExistException>();
        }
    }
}