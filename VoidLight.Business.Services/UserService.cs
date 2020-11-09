using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Business.Authentication;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Infrastructure.Common.Exceptions;

namespace VoidLight.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IJWTService _jWTService;
        private readonly VoidLightDbContext _context;
        private readonly IEmailService _emailService;

        public UserService(IJWTService jwtService, VoidLightDbContext context, IEmailService emailService)
        {
            _jWTService = jwtService;
            _context = context;
            _emailService = emailService;
        }
        public async Task ActivateAccount(string token)
        {
            var email = _jWTService.DecodeRegisterToken(token);

            var user = await FindByEmail(email);

            if (user.IsActivated)
            {
                throw new AccountAlreadyConfirmed();
            }
            user.IsActivated = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task CreateUser(RegisterDto registerDto)
        {
            User user = new User()
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Password = HashingManager.GetHashedPassword(registerDto.Password, registerDto.Username),
                Username = registerDto.Username,
            };
            user.Password = HashingManager.GetHashedPassword(user.Password, user.Username);
            user.IsActivated = false;
            user.WasPasswordForgotten = false;
            user.WasPasswordChanged = false;
            user.AvatarPath = Constants.DEFAULT_IMAGE_USER;
            var token = _jWTService.GenerateRegisterJWT(user);
            await _emailService.SendActivationEmail(user, token);
            user.RoleId = (int)RoleType.Regular;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public IAsyncEnumerable<User> GetAll()
        {
            return _context.Users.AsAsyncEnumerable();
        }

        public async Task ResetPassword(string email, bool isForgotten, string password = null, string newPassword = null)
        {
            if (!isForgotten && string.IsNullOrEmpty(password) && string.IsNullOrEmpty(newPassword))
            {
                throw new InvalidParameterException("Empty password");
            }

            var dbUser = await FindByEmail(email);
            if (isForgotten)
            {
                var randomPassword = GetRandomPassword();
                dbUser.WasPasswordForgotten = true;
                dbUser.WasPasswordChanged = true;
                dbUser.Password = randomPassword;
                await _emailService.SendResetPasswordEmail(dbUser, true);
                dbUser.Password = HashingManager.GetHashedPassword(randomPassword, dbUser.Username);
            }
            else
            {
                if (!dbUser.Password.Equals(HashingManager.GetHashedPassword(password, dbUser.Username)))
                {
                    throw new UnauthorisedException("Your current password is incorrect");
                }

                dbUser.WasPasswordForgotten = false;
                dbUser.WasPasswordChanged = false;
                dbUser.Password = HashingManager.GetHashedPassword(newPassword, dbUser.Username);
                await _emailService.SendResetPasswordEmail(dbUser, false);
            }

            _context.Users.Update(dbUser);
            await _context.SaveChangesAsync();
        }

        private string GetRandomPassword()
        {
            var random = new Random();
            var passwodValue = random.Next(0, 1_000_000);
            var randomPassword = passwodValue.ToString(Constants.PASSWORD_TEMPLATE);

            return randomPassword;
        }

        private async Task<User> FindByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.Email.Equals(email))
                                     .FirstOrDefaultAsync();

            return user ?? throw new UnauthorisedException($"No user with email: {email}");
        }

        public async Task UpdateUser(UserDto userDto)
        {
            User user = new User()
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Password = userDto.Password,
                FullName = userDto.FullName,
                AvatarPath = userDto.AvatarPath,
                Email = userDto.Username
            };
            //await CheckUserFields(user, true);
            var dbUser = await FindByEmail(user.Email);
            dbUser.FullName = user.FullName;
            dbUser.AvatarPath = user.AvatarPath;

            _context.Users.Update(dbUser);
            await _context.SaveChangesAsync();
        }
    }
}
