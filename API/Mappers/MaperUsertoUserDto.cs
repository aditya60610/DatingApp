using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interface;

namespace API.Mappers
{
    public class MaperUsertoUserDto : IMaperUsertoUserDto
    {

        public UserDto MapUsertoUserDto(AppUser user, ITokenService _token)
        {
            return new UserDto (){
                Name = user.UserName,
                Token = _token.CreateToken(user)
            };
        }
    }
}
