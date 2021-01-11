using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class UserPost
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public bool IsShared { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class UserPostConfiguration : IEntityTypeConfiguration<UserPost>
    {
        public void Configure(EntityTypeBuilder<UserPost> builder)
        {
            builder.HasKey(up => new { up.UserId, up.PostId});

            builder
                .HasOne(up => up.User)
                .WithMany(us => us.UserPosts)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(up => up.Post)
                .WithMany(post=>post.UserPosts)
                .HasForeignKey(up => up.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
