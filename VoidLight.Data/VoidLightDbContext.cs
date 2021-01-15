using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoidLight.Data.Entities;

namespace VoidLight.Data
{
    public class VoidLightDbContext : DbContext
    {

        #region Constructor and Config

        public VoidLightDbContext(DbContextOptions<VoidLightDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Game).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryGame).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Friend).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameCategory).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GamePlatform).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GamePublisher).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameUser).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Platform).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserComments).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRole).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebsiteAchievement).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebsiteAchievementUser).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserPlatform).Assembly);

        }

        public object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Save

        /// <summary>
        /// Override of SaveChangesAsync method enables us to write custom code to be executed when SaveChangesAsync is called
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // custom code

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        /// <summary>
        /// Override of SaveChanges method enables us to write custom code to be executed when SaveChanges is called
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // custom code

            var result = base.SaveChanges();

            return result;
        }

        #endregion

        #region Db Sets

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<CategoryGame> CategoryGames { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<GameCategory> GameCategories { get; set; }
        public virtual DbSet<GamePlatform> GamePlatforms { get; set; }
        public virtual DbSet<GamePublisher> GamePublishers { get; set; }
        public virtual DbSet<GameUser> GameUsers { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<UserComments> UserComments { get; set; }
        public virtual DbSet<WebsiteAchievement> WebsiteAchievements { get; set; }
        public virtual DbSet<WebsiteAchievementUser> WebsiteAchievementUsers { get; set; }
        public virtual DbSet<UserPost> UserPosts { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostComment> Comments { get; set; }
        public virtual DbSet<Content> Content { get; set; }
        public virtual DbSet<PostLike> Likes { get; set; }
        public virtual DbSet<UserPlatform> UserPlatforms { get; set; }
        public virtual DbSet<GameAchievement> GameAchievements { get; set; }
        #endregion
    }
}
