using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Business.Services;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Entities;

namespace VoidLight.Testing
{
    public class UserServiceTests
    {
        private readonly VoidLightDbContext _context;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<VoidLightDbContext>().UseInMemoryDatabase(databaseName: "test")
           .Options;
            _context = new VoidLightDbContext(options);

            _userService = new UserService(null,_context,null,null);

            var user1 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "a@g.ro", Username = "user1" };
            var user2 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "b@g.ro", Username = "user2" };
            var user3 = new User() { AccountCreatedDate = DateTime.Now, Country = "Ro", Email = "c@g.ro", Username = "user3" };


            _context.Add(user1);
            _context.Add(user2);
            _context.SaveChanges();
        }
    }
}
