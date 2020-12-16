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
        private readonly ISteamClient _steamClient;


        public FriendService(VoidLightDbContext context, ISteamClient steamClient)
        {
            _context = context;
            _steamClient = steamClient;
        }

        public async Task ConfirmFriendRequest(int initializerId, int receiverId)
        {
            var initializer = await _context.Users.Include(u => u.FriendsList).FirstOrDefaultAsync(u => u.Id == initializerId);
            var receiver = await _context.Users.Include(u => u.FriendsList).FirstOrDefaultAsync(u => u.Id == receiverId);
            var friendRequest = await _context.Friends.FirstOrDefaultAsync(f => f.SelfUserId == initializerId && f.FriendUserId == receiverId);
            friendRequest.IsConfirmed = true;
            _context.Update(friendRequest);
            receiver.FriendsList.Add(new Data.Entities.Friend()
            {
                SelfUser = receiver,
                SelfUserId = receiverId,
                FriendUser = initializer,
                FriendUserId = initializerId,
                IsConfirmed = true
            });
            _context.Update(receiver);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineFriendRequest(int initializerId, int receiverId)
        {
            var friendRequest = await _context.Friends.FirstOrDefaultAsync(f => f.SelfUserId == initializerId && f.FriendUserId == receiverId);
            _context.Remove(friendRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFriends(int initializerId, int receiverId)
        {
            var friendRequest = await _context.Friends.FirstOrDefaultAsync(f => f.SelfUserId == initializerId && f.FriendUserId == receiverId);
            _context.Remove(friendRequest);
            var secondFriendRequest = await _context.Friends.FirstOrDefaultAsync(f => f.SelfUserId == receiverId && f.FriendUserId == initializerId);
            _context.Remove(secondFriendRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetFriendsOfUser(int userId)
        {
            HashSet<UserDto> friends = new HashSet<UserDto>(new UserDtoComparer());

            var dbUser = await _context.Users
                .Include(user => user.FriendsList).ThenInclude(user => user.FriendUser)
                .Include(user => user.Role)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var platform = await _context.Platforms.FirstOrDefaultAsync(platf => platf.Name == "Steam");

            foreach (var friend in dbUser.FriendsList)
            {
                var friendDto = UserMapper.ConvertEntityToDto(friend.FriendUser);
                var userPlatform = await _context.UserPlatforms.FirstOrDefaultAsync(up => up.UserId == friend.FriendUser.Id && up.PlatformId == platform.Id);
                if (userPlatform != null)
                {
                    var game = await _steamClient.GetUserCurrentPlayingGame(userPlatform.LoginToken);
                    friendDto.PlayedGame = game;
                }
                else
                {
                    friendDto.PlayedGame = "Unknown";
                }
                friends.Add(friendDto);
            }

            return friends.ToList();
        }

        public IAsyncEnumerable<UserDto> GetUserFriendRequests(int userId)
        {
            return _context.Friends
                .Include(f => f.SelfUser).ThenInclude(user => user.Role)
                .Where(f=>f.FriendUserId==userId && f.IsConfirmed==false)
                .Select(friend=>UserMapper.ConvertEntityToDto(friend.SelfUser))
                .AsAsyncEnumerable();
        }

        public async Task SendFriendRequest(int selfUserId, int toUserId)
        {
            var selfUser = await _context.Users.Include(u => u.FriendsList).FirstOrDefaultAsync(u => u.Id == selfUserId);
            var toUser = await _context.Users.Include(u => u.FriendsList).FirstOrDefaultAsync(u => u.Id == toUserId);
            selfUser.FriendsList.Add(new Data.Entities.Friend()
            {
                SelfUser = selfUser,
                SelfUserId = selfUserId,
                FriendUser = toUser,
                FriendUserId = toUserId,
                IsConfirmed = false
            });
            _context.Update(selfUser);
            await _context.SaveChangesAsync();
        }
    }
}
