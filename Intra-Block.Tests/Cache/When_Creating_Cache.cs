using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Creating_Cache
    {
        [Test]
        public async Task Should_Initialise_Empty()
        {
            var cache = new Intra_Block.Cache.Cache();
            cache.NumberOfEntries().Should().Be(0);
        }
    }
}