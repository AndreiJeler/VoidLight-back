using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoidLight.Data.Entities
{
    public class WebsiteAchievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WebsiteAchievementUser> Users { get; set; }
    }
    
    public class WebsiteAchievementConfiguration : IEntityTypeConfiguration<WebsiteAchievement>
    {
        public void Configure(EntityTypeBuilder<WebsiteAchievement> builder)
        {
            // Properties
            builder
                .HasKey(ach => ach.Id);
            builder
                .Property(ach => ach.Name)
                .IsRequired();
            builder
                .Property(ach => ach.Description)
                .IsRequired();
            
            // Indexes
            builder
                .HasIndex(nameof(WebsiteAchievement.Name))
                .IsUnique();
            
            // Relations
            builder
                .HasMany(wa => wa.Users)
                .WithOne(wau => wau.WebsiteAchievement)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}