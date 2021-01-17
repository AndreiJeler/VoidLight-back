using System;
using System.Collections.Generic;
using System.Text;
using VoidLight.Data.Entities;
using VoidLight.Data.Business;


namespace VoidLight.Data.Mappers
{
    public static class UserMapper
    {
        /*public User ConvertDtoToEntity(UserDto dto)
        {
            return new User()
            {
                AvatarPath=dto.AvatarPath,
                Email=dto.Email,
                FullName=dto.FullName,
                Id=dto.Id,
                Password=dto.Password,
                Role = 
            }
        }*/

        public static UserDto ConvertEntityToDto(User user)
        {
            return new UserDto()
            {
                AvatarPath = user.AvatarPath,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                Role = user.Role.Name,
                Username = user.Username,
                Age = user.Age
            };
        }
    }
}
