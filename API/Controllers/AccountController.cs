using System.Text;
using System.Security.Cryptography;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using API.Interface;

namespace API.Controllers
{
     public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public ITokenService Token { get; }
        private readonly IMaperUsertoUserDto _userMapper;
        public AccountController(DataContext context, ITokenService token , IMaperUsertoUserDto userMapper)
        {
            this._userMapper = userMapper;
            this.Token = token;
            this._context = context;    
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDTO registerUser){

            if(await UserExist(registerUser.userName)) return BadRequest("Username already exist");

            using (var hmac = new HMACSHA512())
            {
                AppUser user = new AppUser()
                {
                    UserName = registerUser.userName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.password)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                UserDto userDto = _userMapper.MapUsertoUserDto(user,Token);
                
                return userDto;
            }

        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginUserDto loginUserDto){

            AppUser user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == loginUserDto.userName);

            if(user == null) return Unauthorized("Invalid User");

           using var hmac = new HMACSHA512(user.PasswordSalt);

            byte[] ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUserDto.password));

            for(int i=0; i< ComputedHash.Length;i++)
            {
                if(ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            UserDto userDto = _userMapper.MapUsertoUserDto(user,Token);
                
            return userDto;

        }



        private async Task<bool> UserExist( string username)
        {
                return  await _context.Users.AnyAsync(x=> x.UserName.Trim().ToLower() == username.Trim().ToLower());
        }
    }
}
