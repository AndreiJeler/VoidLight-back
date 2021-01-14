using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;

namespace VoidLight.Data
{
    public class VoidLightDbConfiguration
    {
        private readonly VoidLightDbContext _context;

        public VoidLightDbConfiguration(VoidLightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is called at startup to ensure the database is created and to seed any data that is required on a first run (static data for example).
        /// </summary>
        public void Seed()
        {
            _context.Database.EnsureCreated();

            _context.Database.Migrate();


            if (!_context.UserRoles.Any()) AddRoles();

            if (!_context.Platforms.Any()) AddPlatforms();


            // seed functionality

             _context.SaveChanges();
        }


        private void AddRoles()
        {
            _context.Add(new UserRole
            {
                Id = (int)RoleType.Admin,
                Name = "Admin",
            });

            _context.Add(new UserRole
            {
                Id = (int)RoleType.Streamer,
                Name = "Streamer",
            });

            _context.Add(new UserRole
            {
                Id = (int)RoleType.Regular,
                Name = "Regular",
            });
        }

        private void AddPlatforms()
        {
            _context.Add(new Platform()
            {
                Name = "Steam",
                PicturePath = Constants.DEFAULT_IMAGE_USER
            });
            _context.Add(new Platform()
            {
                Name = "Discord",
                PicturePath = Constants.DEFAULT_IMAGE_USER
            });
        }
    }
}
