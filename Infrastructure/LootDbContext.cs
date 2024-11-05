using Infrastructure.Models;
using Infrastructure.Models.GuildApplications;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class LootDbContext : IdentityDbContext<BlizzUser>
    {
        public LootDbContext(DbContextOptions<LootDbContext> options)
            : base(options)
        {            
        }

        public DbSet<GuildApplication> Applications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildApplication>()
                .HasOne(a => a.Applicant)
                .WithOne(a => a.GuildApplication);

            base.OnModelCreating(modelBuilder);
        }
    }
}
