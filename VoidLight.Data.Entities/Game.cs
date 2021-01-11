using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public GamePublisher Publisher { get; set; }
        //public int PublisherId { get; set; }
        public ICollection<GameUser> GameUsers { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }
        public ICollection<CategoryGame> Categories { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<GameAchievement> GameAchievements { get; set; }

    }

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            // Properties
            builder
                .HasKey(game => game.Id);
            builder
                .Property(game => game.Name)
                .IsRequired();
            builder
                .Property(user => user.Description)
                .IsRequired();

            // Indexes
            /*builder
                .HasIndex(nameof(Game.Name))
                .IsUnique();*/

            // Relations
            /*builder
                .HasOne(game => game.Publisher)
                .WithMany(gp => gp.Games)
                .HasForeignKey(game => game.PublisherId);*/
            builder
                .HasMany(g => g.GameUsers)
                .WithOne(gu => gu.Game)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(g => g.GamePlatforms)
                .WithOne(gp => gp.Game)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(g => g.GameAchievements)
                .WithOne(gp => gp.Game)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(g => g.Categories)
                .WithOne(cat => cat.Game)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
