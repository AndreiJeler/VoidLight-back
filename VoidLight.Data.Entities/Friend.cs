using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoidLight.Data.Entities;

namespace VoidLight.Data.Entities
{
    public class Friend
    {
        public int SelfUserId { get; set; }
        public User SelfUser { get; set; }
        public int FriendUserId { get; set; }
        public User FriendUser { get; set; }
    }
    
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            // Properties
            builder
                .HasKey(friend => new {friend.SelfUserId, friend.FriendUserId});
            builder
                .Property(friend => friend.SelfUser)
                .IsRequired();
            builder
                .Property(friend => friend.FriendUser)
                .IsRequired();
            
            // Relations
            builder
                .HasOne(friend => friend.SelfUser)
                .WithMany(su => su.SelfFriends)
                .HasForeignKey(friend => friend.SelfUserId);
            builder
                .HasOne(friend => friend.FriendUser)
                .WithMany(fu => fu.FriendOfList)
                .HasForeignKey(friend => friend.FriendUserId);
        }
    }
}