using System;
using System.Collections.Generic;
using System.Text;

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

            // seed functionality

            // _context.SaveChanges();
        }
    }
}
