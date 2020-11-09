using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoidLight.Data.Entities
{
    public class WebsiteAchievementUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int WebsiteAchievementId { get; set; }
        public WebsiteAchievement WebsiteAchievement { get; set; }
    }

    public class WebsiteAchievementUserConfiguration : IEntityTypeConfiguration<WebsiteAchievementUser>
    {
        public void Configure(EntityTypeBuilder<WebsiteAchievementUser> builder)
        {
            // Properties
            builder
                .HasKey(wau => new {wau.UserId, wau.WebsiteAchievementId});

            // Relations
            builder
                .HasOne(wau => wau.User)
                .WithMany(user => user.WebsiteAchievements)
                .HasForeignKey(wau => wau.UserId);
            builder
                .HasOne(wau => wau.WebsiteAchievement)
                .WithMany(wa => wa.Users)
                .HasForeignKey(wau => wau.WebsiteAchievementId);
        }
    }
}