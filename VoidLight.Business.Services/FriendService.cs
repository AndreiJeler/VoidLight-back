using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Mappers;

namespace VoidLight.Business.Services
{
    public class FriendService : IFriendService
    {
        private readonly VoidLightDbContext _context;

        public FriendService(VoidLightDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetFriendsOfUser(int userId)
        {
            HashSet<UserDto> friends = new HashSet<UserDto>(new UserDtoComparer());

            var dbUser = await _context.Users
                .Include(user => user.SelfFriends).ThenInclude(user => user.FriendUser)
                .Include(user => user.FriendOfList).ThenInclude(user => user.SelfUser)
                .Include(user=>user.Role)
                .FirstOrDefaultAsync(user => user.Id == userId);

            foreach (var friend in dbUser.SelfFriends)
            {
                friends.Add(UserMapper.ConvertEntityToDto(friend.FriendUser));
            }
            foreach (var friend in dbUser.FriendOfList)
            {
                friends.Add(UserMapper.ConvertEntityToDto(friend.SelfUser));
            }

            return friends.ToList();
        }
    }
}
