using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class GameUser
    {
        public Game Game { get; set; }
        public int GameId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public bool IsFavourite { get; set; }
    }

    public class GameUserConfiguration : IEntityTypeConfiguration<GameUser>
    {
        public void Configure(EntityTypeBuilder<GameUser> builder)
        {
            // Properties
            builder
                .HasKey(gu => new { gu.GameId, gu.UserId});
            

            // Relations
            builder
                .HasOne(gu => gu.Game)
                .WithMany(game => game.GameUsers)
                .HasForeignKey(gu => gu.GameId);
            builder
                .HasOne(gu => gu.User)
                .WithMany(user => user.GameUsers)
                .HasForeignKey(gu => gu.UserId);
        }
    }
}
