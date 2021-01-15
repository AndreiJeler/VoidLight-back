using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Entities
{
    public class Lobby
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Game? Game { get; set; }
        public int GameId { get; set; }
        public bool HasStarted { get; set; }
        public ICollection<UserLobby> UserLobbies { get; set; }
    }

    public class LobbyConfiguration : IEntityTypeConfiguration<Lobby>
    {
        public void Configure(EntityTypeBuilder<Lobby> builder)
        {
            // Properties
            builder
                .HasKey(lobby => lobby.Id);


            builder
               .HasOne(l => l.Game)
               .WithMany(game => game.Lobbies)
               .HasForeignKey(l => l.GameId);

            builder
               .HasMany(l => l.UserLobbies)
               .WithOne(lob => lob.Lobby)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
