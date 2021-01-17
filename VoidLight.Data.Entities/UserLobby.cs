using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class UserLobby
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Lobby Lobby { get; set; }
        public int LobbyId { get; set; }
        public bool IsInitializer { get; set; }
    }

    public class UserLobbyConfiguration : IEntityTypeConfiguration<UserLobby>
    {
        public void Configure(EntityTypeBuilder<UserLobby> builder)
        {
            // Properties
            builder
                .HasKey(gu => new { gu.UserId, gu.LobbyId });


            // Relations
            builder
                .HasOne(ul => ul.User)
                .WithMany(user => user.UserLobbies)
                .HasForeignKey(ul => ul.UserId);
            builder
                .HasOne(ul => ul.Lobby)
                .WithMany(lobby => lobby.UserLobbies)
                .HasForeignKey(ul => ul.LobbyId);
        }
    }
}
