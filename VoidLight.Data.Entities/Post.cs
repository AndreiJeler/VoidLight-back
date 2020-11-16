using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Content> Content { get; set; }
        public ICollection<PostComment> Comments { get; set; }
        public ICollection<PostLike> Likes { get; set; }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(pst => pst.Id);

            builder
                .HasMany(pst => pst.Content);

            builder
                .HasMany(pst => pst.Comments)
                .WithOne(c => c.Post);

            builder
                .HasMany(pst => pst.Likes)
                .WithOne(l => l.Post);
        }
    }
}
