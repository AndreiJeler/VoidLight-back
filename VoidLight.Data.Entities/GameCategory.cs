using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class GameCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryGame> CategoryGames { get; set; }

    }

    public class GameCategoryConfiguration : IEntityTypeConfiguration<GameCategory>
    {
        public void Configure(EntityTypeBuilder<GameCategory> builder)
        {
            // Properties
            builder
                .HasKey(gc => gc.Id);
            builder
                .Property(gc => gc.Name)
                .IsRequired();

            // Indexes
            builder
                .HasIndex(nameof(GameCategory.Name))
                .IsUnique();

            // Relations
            builder
                .HasMany(cat => cat.CategoryGames)
                .WithOne(catg => catg.GameCategory)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
