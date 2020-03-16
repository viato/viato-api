using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public DbSet<Post> Posts { get; set; }

        public DbSet<Contribution> Contributions { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<StagedContribution> StagedContributions { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        protected void AddTimestamps()
        {
            var entries = ChangeTracker
               .Entries()
               .Where(e => e.Entity is AuditableEntityBase && (
                       e.State == EntityState.Added
                       || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableEntityBase)entityEntry.Entity).UpdatedDate = DateTimeOffset.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntityBase)entityEntry.Entity).CreatedDate = DateTimeOffset.UtcNow;
                }
            }
        }

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

            modelBuilder.Entity<Organization>()
                .HasIndex(x => x.Domain)
                .IsUnique();

            modelBuilder.Entity<Contribution>()
                .HasIndex(x => x.TorTokenId)
                .IsUnique();
        }
    }
}
