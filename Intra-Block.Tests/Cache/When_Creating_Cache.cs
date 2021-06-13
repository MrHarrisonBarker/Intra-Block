using System.Threading.Tasks;
using FluentAssertions;
using Intra_Block.Cache;
using NUnit.Framework;

namespace Intra_Block.Tests.Cache
{
    [TestFixture]
    public class When_Creating_Cache
    {
        private Administratum Administratum;
        
        [SetUp]
        public void SetUp()
        {
            Administratum = new Administratum();
        }
        
        [Test]
        public async Task Should_Initialise_Empty()
        {
            var cache = new Intra_Block.Cache.Cache(Administratum);
            cache.NumberOfEntries().Should().Be(0);
        }
    }
}