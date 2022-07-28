using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class AccountController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
        {
            return BadRequest(new {Response = "Username is taken"});
        }


        using var hmac = new HMACSHA512();

        var user = new User
        {
            UserName = registerDto.UserName.ToLower(),
            Password = registerDto.Password,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
            Email = registerDto.Email.ToLower(),
            Avatar = registerDto.Avatar
        };

        _userRepository.AddUserAsync(user);
        var result = await _userRepository.SaveAllAsync();
        if (!result)
        {
            return BadRequest(new {Response = "User didn't saved"});
        }

        return new UserDto {UserName = user.UserName, Token = _tokenService.CreateToken(user)};
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.UserName);
        if (user == null)
        {
            return Unauthorized(new {Response = "User does not exist"});
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized(new {Response = "Invalid password"});
            }
        }

        return new UserDto {UserName = user.UserName, Token = _tokenService.CreateToken(user)};
    }

    private async Task<bool> UserExists(string username)
    {
        var result = await _userRepository.GetUserByUsernameAsync(username);
        return result != null;
    }
}
