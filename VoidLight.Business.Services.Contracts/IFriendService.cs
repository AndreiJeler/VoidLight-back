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
    }
}
