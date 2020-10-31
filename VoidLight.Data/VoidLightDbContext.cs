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

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(Change).Assembly);
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

        public virtual DbSet<Example> Examples { get; set; }

        public virtual DbSet<User> Users { get; set; }

        #endregion
    }
}
