using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.Interface
{
    public interface IMaperUsertoUserDto
    {
       UserDto MapUsertoUserDto(AppUser user, ITokenService _token);
    }
}