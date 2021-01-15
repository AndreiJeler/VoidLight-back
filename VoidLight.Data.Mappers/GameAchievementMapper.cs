using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;

namespace VoidLight.Data.Mappers
{
    public static class GameAchievementMapper
    {
        public static GameAchievementDto ConvertEntityToDto(GameAchievement achievement)
        {
            return new GameAchievementDto()
            {
                UserId = achievement.UserId,
                GameId = achievement.GameId,
                AchievementName = achievement.Description,
                TimeAchieved = achievement.TimeAchieved,
                //GameName = achievement.Game.Name,
                Icon = achievement.Icon,
                //UserName = achievement.User.Username
            };
        }
    }
}
