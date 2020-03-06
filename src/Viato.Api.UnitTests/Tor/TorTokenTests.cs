using System;
using Viato.Api.Tor;
using Xunit;

namespace Viato.Api.UnitTests.Tor
{
    public class TorTokenTests
    {
        [Theory]
        [InlineData("42A210AD-B622-4FD1-863E-30814C8E4C9B", 10, 20, 20.456)]
        [InlineData("5429988B-B187-4433-BA9B-2489A9AC1320", 100, 200, 44120.456)]
        [InlineData("336BF808-1EC7-4C94-9B0A-DB18AE17E973", long.MaxValue, long.MaxValue, 1_000_000_000_000_000)]
        public void GetTokenShouldReturnValidToken(string id, long sourceOrgId, long destOrgId, decimal amount)
        {
            // Arrange
            var ecKey = new ECKey();
            var torToken = new TorToken(Guid.Parse(id), sourceOrgId, destOrgId, amount, null);

            // Act
            var token = torToken.Protect(ecKey.GetPrivateKey());

            // Assert
            var parsedToken = TorToken.Parse(token);
            Assert.True(token.Length <= 500 /* just make sure that we don't have huge token */);
            Assert.True(parsedToken.Verify(ecKey.GetPublicKey()));
            Assert.Equal(torToken.SourceOrgId, parsedToken.SourceOrgId);
            Assert.Equal(torToken.DestinationOrgId, parsedToken.DestinationOrgId);
            Assert.Equal(torToken.Amount, parsedToken.Amount);
        }
    }
}
