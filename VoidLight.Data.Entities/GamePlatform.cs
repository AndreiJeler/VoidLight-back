using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class GamePlatform
    {
        public Game Game { get; set; }
        public int GameId { get; set; }
        public Platform Platform { get; set; }
        public int PlatformId { get; set; }

    }

    public class GamePlatformConfiguration : IEntityTypeConfiguration<GamePlatform>
    {
        public void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            // Properties
            builder
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            // Indexes
            builder
                .HasIndex(nameof(GamePlatform.Name))
                .IsUnique();

            // Relations
            builder
                .HasOne(gp => gp.Game)
                .WithMany(game => game.GamePlatforms)
                .HasForeignKey(gp => gp.GameId);
            builder
                .HasOne(gp => gp.Platform)
                .WithMany(plat => plat.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformId);
        }
    }
}
