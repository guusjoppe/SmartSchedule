using Boilerplate.Logging.Utility;
using Xunit;

namespace Boilerplate.Logging.Tests
{
    public class TypeNameTrieTests
    {
        [Theory]
        [InlineData("a", false, -1)]
        [InlineData("a.b", true, 5)]
        [InlineData("a.b.X", true, 5)]
        [InlineData("a.b.c", true, 7)]
        [InlineData("a.b.c.X", true, 7)]
        [InlineData("a.b.c.d", true, 9)]
        [InlineData("a.b.c.d.X", true, 9)]
        public void TestTrie(string prefix, bool shouldMatch, int shouldBeValue)
        {
            var trie = new TypeNameTrie<int>
            {
                { "a.b", 5 },
                { "a.b.c", 7 },
                { "a.b.c.d", 9 },
            };
            
            Assert.Equal(shouldMatch, trie.GetLongestMatch(prefix, out var value));
            if (shouldMatch)
            {
                Assert.Equal(shouldBeValue, value);
            }
        }
    }
}
