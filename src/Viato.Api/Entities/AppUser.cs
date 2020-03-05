using Microsoft.AspNetCore.Identity;

namespace Viato.Api.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public string DomainName { get; set; }
        public bool DnsVerified { get; set; }
    }

    public enum AppUserRole
    {
        SourceOrg,
        Contributor,
        DestinationOrg,
    }
}
