using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class GameAchievementDto
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string GameName { get; set; }
        public string UserName { get; set; }
        public string AchievementName { get; set; }
        public string Icon { get; set; }
        public DateTime TimeAchieved { get; set; }
    }
}
