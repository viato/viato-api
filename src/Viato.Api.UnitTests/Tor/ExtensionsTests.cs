using Viato.Api.Tor;
using Xunit;

namespace Viato.Api.UnitTests.Tor
{
    public class ExtensionsTests
    {
        [Theory]
        [InlineData("0x111", "0x111")]
        [InlineData("111", "0x111")]
        [InlineData("222", "0x222")]
        public void Add0xShouldAdd0x(string str, string expected)
        {
            // Act
            var with0x = str.Add0x();

            // Arrange
            Assert.Equal(expected, with0x);
        }

        [Theory]
        [InlineData("0x111", "111")]
        [InlineData("0x222", "222")]
        [InlineData("222", "222")]
        public void Remove0xShouldRemov0x(string str, string expected)
        {
            // Act
            var without0x = str.Remove0x();

            // Arrange
            Assert.Equal(expected, without0x);
        }
    }
}
