using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoidLight.Data.Entities
{
    public class UserComments
    {
        public int Id { get; set; }
        public int CommentedUserId { get; set; }
        public User CommentedUser { get; set; }
        public string CommentText { get; set; }
    }

    public class UserCommentsConfiguration : IEntityTypeConfiguration<UserComments>
    {
        public void Configure(EntityTypeBuilder<UserComments> builder)
        {
            // Properties
            builder
                .HasKey(user => user.Id);
            builder
                .Property(user => user.CommentedUserId)
                .IsRequired();
            builder
                .Property(user => user.CommentText)
                .IsRequired();

            // Relations
            builder
                .HasOne(uc => uc.CommentedUser)
                .WithMany(user => user.ProfileComments)
                .HasForeignKey(uc => uc.CommentedUserId);
        }
    }
}