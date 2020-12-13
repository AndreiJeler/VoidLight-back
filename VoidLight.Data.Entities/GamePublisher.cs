using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class GamePublisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<Game> Games { get; set; }
    }

    public class GamePublisherConfiguration : IEntityTypeConfiguration<GamePublisher>
    {
        public void Configure(EntityTypeBuilder<GamePublisher> builder)
        {
            // Properties
            builder
                .HasKey(publisher => publisher.Id);
            builder
                .Property(publisher => publisher.Name)
                .IsRequired();
            builder
                .Property(publisher => publisher.Url)
                .IsRequired();

            // Indexes
            builder
                .HasIndex(nameof(GamePublisher.Name))
                .IsUnique();

            // Relations
           /* builder
               .HasMany(publisher => publisher.Games)
               .WithOne(game => game.Publisher)
               .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
