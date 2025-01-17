using DATINGAPP.Data;
using DATINGAPP.DTOs;
using DATINGAPP.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DATINGAPP.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (await UserExists(register.Username))
            {
                return BadRequest("Username is taken");
            }
            return Ok();
            //using var hmac = new HMACSHA256();

            //var user = new AppUser
            //{
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            //    UserName = register.Username,
            //    PasswordSalt = hmac.Key
            //};
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            //return new UserDto
            //{
            //    Token = _tokenService.CreateToken(user),
            //    Username = user.UserName
            //};
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.UserName.ToLower() == login.Username.ToLower()
            );
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHmac = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computedHmac.Length; i++)
            {
                if (computedHmac[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
