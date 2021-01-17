using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class UserPlatform
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Platform Platform { get; set; }
        public int PlatformId { get; set; }
        public string LoginToken { get; set; }
        public string KnownAs { get; set; }
        public string LoginId { get; set; }
    }

    public class UserPlatformConfiguration : IEntityTypeConfiguration<UserPlatform>
    {
        public void Configure(EntityTypeBuilder<UserPlatform> builder)
        {

            builder
                .HasKey(gu => new { gu.UserId, gu.PlatformId});

            builder
                 .HasOne(up => up.User)
                 .WithMany(us => us.UserPlatforms)
                 .HasForeignKey(up => up.UserId);
            builder
                  .HasOne(up => up.Platform)
                  .WithMany(us => us.UserPlatforms)
                  .HasForeignKey(up => up.PlatformId);
        }
    }
}
