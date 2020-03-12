using System.Threading.Tasks;
using Viato.Api.Entities;
using Viato.Api.Services;
using Xunit;

namespace Viato.Api.UnitTests.Services
{
    public class DnsProofServiceTests
    {
        private readonly DnsProofService _systemUnderTest = new DnsProofService();

        [Fact]
        public async Task VerifyProofShouldReturnFalseForUnverifiedDomain()
        {
            // Arrange
            var org = new Organization()
            {
                Id = 1010,

                // When facebook joins viato this test is going to fail and it will be nice suprise to us
                Domain = "facebook.com",
            };

            // Act
            var isVerified = await _systemUnderTest.VerifyProofAsync(org);

            // Assert
            Assert.False(isVerified);
        }

        [Fact]
        public async Task VerifyProofShouldReturnTrueForVerifiedDomain()
        {
            // Arrange
            var org = new Organization()
            {
                Id = -123456789,

                // if this test, starts failing, it means that we no longer own this domain
                Domain = "via-to.com",
            };

            // Act
            var isVerified = await _systemUnderTest.VerifyProofAsync(org);

            // Assert
            Assert.True(isVerified);
        }
    }
}
