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
        public Task<string> ConfirmFriendRequest(int initializerId, int receiverId);
        public Task<string> DeclineFriendRequest(int initializerId, int receiverId);
        public IAsyncEnumerable<UserDto> GetUserFriendRequests(int userId);
        public Task DeleteFriends(int initializerId, int receiverId);
        public Task RemoveFriendRequest(int initializerId, int receiverId);
        public Task<int> GetFriendType(int initializerId, int receiverId);

    }
}
