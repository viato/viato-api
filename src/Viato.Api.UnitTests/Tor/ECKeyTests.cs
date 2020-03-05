using Viato.Api.Tor;
using Xunit;

namespace Viato.Api.UnitTests.Tor
{
    public class ECKeyTests
    {
        [Fact]
        public void TestED25519Signature()
        {
            // Arrange
            var ecKey = new ECKey();
            byte[] msg = "test".GetBytes();

            // Act
            var signature = ecKey.Sign(msg);

            // Assert
            Assert.True(ECKey.Verify(msg, signature, ecKey.GetPublicKey()));
        }
    }
}
