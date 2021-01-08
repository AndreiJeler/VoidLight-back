using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Entities;
using Xunit;

namespace VoidLight.Testing
{
    public class GameServiceTests
    {
        private readonly VoidLightDbContext _context;
        private readonly IGameService _gameService;

        public GameServiceTests()
        {
            var options = new DbContextOptionsBuilder<VoidLightDbContext>().UseInMemoryDatabase(databaseName: "test")
           .Options;
            _context = new VoidLightDbContext(options);

            _gameService = new GameService(_context);

            var user1 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "a@g.ro", Username = "user1" };
            var user2 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "b@g.ro", Username = "user2" };
            var user3 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "c@g.ro", Username = "user3" };


            var platform = new Platform()
            {
                Name = "Steam"
            };

            var game1 = new Game()
            {
                GameAchievements = new List<GameAchievement>(),
                Description = "Description 1",
                Name = "Game 1"
            };

            var game1Platform = new GamePlatform() { Game = game1, Platform = platform, AppId = "1" };
            game1.GamePlatforms = new List<GamePlatform>() { game1Platform };
            game1.GameAchievements.Add(new GameAchievement() { Description = "Ach 1", Game = game1, User = user1 });
            game1.GameAchievements.Add(new GameAchievement() { Description = "Ach 1", Game = game1, User = user3 });
            game1.GameUsers = new List<GameUser>() { new GameUser() { Game = game1, User = user1 }, new GameUser() { Game = game1, User = user3 } };

            var game2 = new Game()
            {
                GameAchievements = new List<GameAchievement>(),
                Description = "Description 2",
                Name = "Game 2"
            };

            var game2Platform = new GamePlatform() { Game = game2, Platform = platform, AppId = "2" };
            game2.GamePlatforms = new List<GamePlatform>() { game2Platform };
            game2.GameAchievements.Add(new GameAchievement() { Description = "Ach 2", Game = game2, User = user2 });
            game2.GameUsers = new List<GameUser>() { new GameUser() { Game = game2, User = user2 } };


            _context.Add(game1);
            _context.Add(game2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task TestGetSuccesfully()
        {
            var user = await _context.Users.Include(u=>u.GameUsers).ThenInclude(g=>g.Game).ThenInclude(g=>g.GameAchievements).FirstOrDefaultAsync(u => u.Username == "user1");
            Assert.Equal(1, user.GameUsers.Count);
            var enumerator = user.GameUsers.GetEnumerator();
            enumerator.MoveNext();
            var game = enumerator.Current.Game;
            Assert.Equal("Game 1", game.Name);
            Assert.Equal("Description 1", game.Description);
            Assert.Equal(2, game.GameAchievements.Count);
        }

        [Fact]
        public async Task TestGetFail()
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Name == "Game 4");
            Assert.Null(game);
        }

        [Fact]
        public async Task AddGameSuccesfully()
        {
            var game = new Game() { Name = "GG", Description = "dd" };
            Assert.Equal("GG", game.Name);
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            var games = await _context.Games.ToListAsync();
            Assert.Equal(3, games.Count);
            Assert.Contains(game, games);
            var dbGame = await _context.Games.FirstOrDefaultAsync(g => g.Name == "GG");
            Assert.Equal("dd",dbGame.Description);
            _context.Remove(dbGame);
            await _context.SaveChangesAsync();
            games = await _context.Games.ToListAsync();
            Assert.DoesNotContain(game, games);
        }
    }
}
