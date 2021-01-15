using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Data.Business
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFavourite { get; set; }
        public string Icon { get; set; }
        /*public int AchievementsTotal { get; set; }
        public int AchievementsAcquired { get; set; }*/
        public double TimePlayed { get; set; }

    }
}
