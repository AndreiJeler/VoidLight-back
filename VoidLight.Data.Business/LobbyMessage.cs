using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class LobbyMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserIcon { get; set; }
        public int UserId { get; set; }
    }
}
