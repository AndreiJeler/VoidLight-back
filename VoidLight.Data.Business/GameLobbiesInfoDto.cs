using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class GameLobbiesInfoDto
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameIcon { get; set; }
        public int NrLobbies { get; set; }
    }
}
