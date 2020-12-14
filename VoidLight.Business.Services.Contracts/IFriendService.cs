using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;

namespace VoidLight.Business.Services.Contracts
{
    public interface IFriendService
    {
        public Task<List<UserDto>> GetFriendsOfUser(int userId);
        public Task SendFriendRequest(int selfUserId, int toUserId);
        public Task ConfirmFriendRequest(int initializerId, int receiverId);
        public Task DeclineFriendRequest(int initializerId, int receiverId);
        public IAsyncEnumerable<UserDto> GetUserFriendRequests(int userId);
        public Task DeleteFriends(int initializerId, int receiverId);
    }
}
