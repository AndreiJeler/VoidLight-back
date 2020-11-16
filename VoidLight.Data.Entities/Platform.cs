using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicturePath { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }


    }

    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            // Properties
            builder
                .HasKey(plat => plat.Id);
            builder
                .Property(plat => plat.Name)
                .IsRequired();
            builder
                .Property(plat => plat.PicturePath)
                .IsRequired();

            // Indexes
            builder
                .HasIndex(nameof(Platform.Name))
                .IsUnique();

            // Relations
            builder
                .HasMany(g => g.GamePlatforms)
                .WithOne(gp => gp.Platform)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
