using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VoidLight.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public string Country { get; set; }
        public bool IsActivated { get; set; }
        public bool WasPasswordForgotten { get; set; }
        public bool WasPasswordChanged { get; set; }
        public UserRole Role { get; set; }
        public int RoleId { get; set; }
        public ICollection<UserComments> ProfileComments { get; set; }
        public ICollection<WebsiteAchievementUser> WebsiteAchievements { get; set; }
        public ICollection<Friend> FriendsList { get; set; }
        public ICollection<GameUser> GameUsers { get; set; }
        public ICollection<UserPost> UserPosts { get; set; }
        public ICollection<PostLike> UserPostLikes { get; set; }
        public ICollection<UserPlatform> UserPlatforms { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Properties
            builder
                .HasKey(user => user.Id);
           /* builder
                .Property(user => user.Email)
                .IsRequired();*/
            builder
                .Property(user => user.Username)
                .IsRequired();
            /* builder
                 .Property(user => user.Password)
                 .IsRequired(); // TODO: The password should not be required once login with Google/Facebook/etc is implemented*/
            /* builder
                 .Property(user => user.LoginService);*/
            /*builder
                .Property(user => user.Gender)
                .IsRequired();
            builder
                .Property(user => user.BirthDate)
                .IsRequired();
            builder
                .Property(user => user.AccountCreatedDate)
                .IsRequired();
            builder
                .Property(user => user.Country)
                .IsRequired();*/

            // Indexes
            builder
                .HasIndex(nameof(User.Email))
                .IsUnique();
            builder
                .HasIndex(nameof(User.Username))
                .IsUnique();

            // Relations
            builder
                .HasOne(user => user.Role);
            builder
                .HasMany(user => user.ProfileComments)
                .WithOne(uc => uc.CommentedUser)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(user => user.WebsiteAchievements)
                .WithOne(wa => wa.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(su => su.FriendsList)
                .WithOne(friend => friend.SelfUser)
                .OnDelete(DeleteBehavior.NoAction);
/*            builder
                .HasMany(fu => fu.FriendOfList)
                .WithOne(friend => friend.FriendUser)
                .OnDelete(DeleteBehavior.NoAction);*/
            builder
                .HasMany(u => u.GameUsers)
                .WithOne(gu => gu.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(user => user.UserPlatforms)
                .WithOne(uc => uc.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}