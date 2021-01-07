using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class GameAchievement
    {
        public Game Game { get; set; }
        public int GameId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime TimeAchieved { get; set; }
    }

    public class GameAchievementConfiguration : IEntityTypeConfiguration<GameAchievement>
    {
        public void Configure(EntityTypeBuilder<GameAchievement> builder)
        {
            // Properties
            builder
                .HasKey(gu => new { gu.GameId, gu.UserId, gu.Description });


            // Relations
            builder
                .HasOne(gu => gu.Game)
                .WithMany(game => game.GameAchievements)
                .HasForeignKey(gu => gu.GameId);
            builder
                .HasOne(gu => gu.User)
                .WithMany(user => user.Achievements)
                .HasForeignKey(gu => gu.UserId);
        }
    }
}
