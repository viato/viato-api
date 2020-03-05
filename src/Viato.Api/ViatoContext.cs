using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Entities;

namespace Viato.Api
{
    public class ViatoContext : IdentityDbContext<AppUser, AppRole, long>
    {
        public ViatoContext(DbContextOptions<ViatoContext> options) : base(options)
        {

        }

        public DbSet<ContributionPipeline> ContributionPipelines { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
    }
}
