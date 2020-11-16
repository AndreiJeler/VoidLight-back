using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class PostComment
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string Text { get; set; }
    }
    public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(pc => new { pc.UserId, pc.PostId });

            builder
               .HasOne(up => up.Post)
               .WithMany(pst => pst.Comments)
               .HasForeignKey(up => up.PostId)
               .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
