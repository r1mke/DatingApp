using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // POST: /api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDto)
    {
        if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
        .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt); // kreiramo hmac objekt koristeci salt iz baze

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // koristimo isti alogritam(hmacsha512) i isti salt za izračunavanje hash-a

        for(int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            // poredimo svaki byte iz hash-a sa svakim byte-om iz baze
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExist(string username) => await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()) ;
}