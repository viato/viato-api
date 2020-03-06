using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Viato.Api.Entities;

namespace Viato.Api
{
    public class ViatoContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public ViatoContext(DbContextOptions<ViatoContext> options)
            : base(options)
        {
        }

        public DbSet<ContributionPipeline> ContributionPipelines { get; set; }

        public DbSet<Contribution> Contributions { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContributionPipeline>()
                .HasOne(x => x.SourceOrganizaton)
                .WithOne()
                .HasForeignKey<ContributionPipeline>(p => p.SourceOrganizationId);
            modelBuilder.Entity<ContributionPipeline>()
                .HasOne(x => x.DestinationOrganization)
                .WithOne()
                .HasForeignKey<ContributionPipeline>(p => p.DestinationOrganizationId);
        }
    }
}
