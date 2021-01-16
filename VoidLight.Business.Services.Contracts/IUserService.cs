using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IUserService
    {
        Task CreateUser(RegisterDto registerDto);
        Task ActivateAccount(string token);
        Task ResetPassword(string email, bool isForgotten, string password = null, string newPassword = null);
        Task UpdateUser(string userJson, IFormFileCollection files);
        Task<User> FindById(int id);  //TODO: de schimbat la dto
        IAsyncEnumerable<User> GetAll();
        Task<UserDto> GetById(int id);
        Task SteamRegister(string steamId, string username);
        Task<int> GetUserIdSteamLogin(string steamId, string username);
        IAsyncEnumerable<UserDto> GetUsersWithName(string name);
        public Task<int> DiscordAuthentication(string code);
        public Task<string> GetPlatformUser(int id, string platform);
        public Task<UserDto> SteamSync(int userId, string steamId, string username);
        public Task RefreshGames(int userId);
    }
}
