using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Viato.Api.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public virtual List<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
