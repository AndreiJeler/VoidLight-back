using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int ParticipantsNr { get; set; }
        public List<UserDto> Users { get; set; } //de creat un lobby user care sa contina discord things
        public bool HasStarted { get; set; }
    }
}
