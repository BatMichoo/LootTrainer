using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Models.GuildApplications
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<GuildApplication>
    {
        public void Configure(EntityTypeBuilder<GuildApplication> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Applicant)
                .WithOne(a => a.GuildApplication);

            builder.Property(a => a.State)
                .HasConversion<int>();
        }
    }
}
