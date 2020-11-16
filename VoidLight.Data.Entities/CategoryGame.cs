using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class CategoryGame
    {
        public Game Game { get; set; }
        public int GameId { get; set; }
        public GameCategory GameCategory { get; set; }
        public int CategoryId { get; set; }
    }

    public class CategoryGameConfiguration : IEntityTypeConfiguration<CategoryGame>
    {
        public void Configure(EntityTypeBuilder<CategoryGame> builder)
        {
            // Properties
            builder
                .HasKey(cg => new { cg.GameId, cg.CategoryId});

            // Relations
            builder
                .HasOne(gc => gc.Game)
                .WithMany(game => game.Categories)
                .HasForeignKey(gc => gc.GameId);
            builder
                .HasOne(gc => gc.GameCategory)
                .WithMany(categ => categ.CategoryGames)
                .HasForeignKey(gc => gc.CategoryId);
        }
    }
}
