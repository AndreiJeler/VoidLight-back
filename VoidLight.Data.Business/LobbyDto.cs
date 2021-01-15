using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business.Discord;

namespace VoidLight.Data.Business
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int ParticipantsNr { get; set; }
        public IEnumerable<DiscordUserDto> Users { get; set; }
        public DiscordUserDto Initializer { get; set; }
        public bool HasStarted { get; set; }
    }
}
