using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class AccountController : BaseApiController
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository, ITokenService tokenService,
        IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _groupRepository = groupRepository;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
        {
            return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "User already exists"});
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

        await _userRepository.AddUserAsync(user);

        var result = await _userRepository.SaveAllAsync();
        if (!result)
        {
            return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "User did not saved"});
        }

        return CreatedAtAction(nameof(Register),
            new TokenDto {UserName = user.UserName, Token = _tokenService.CreateToken(user)});
    }

    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.UserName);
        if (user == null)
        {
            return Unauthorized(new ResponseDto
            {
                Status = ResponseStatus.Error,
                Message = "User does not exist",
            });
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

        return Ok(new TokenDto {UserName = user.UserName, Token = _tokenService.CreateToken(user)});
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("add_to_group")]
    public async Task<ActionResult<TokenDto>> AddToGroup(AddToGroupDto addToGroupDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(addToGroupDto.UserName);
        if (user == null)
        {
            return Unauthorized(new ResponseDto
            {
                Status = ResponseStatus.Error,
                Message = "User does not exist",
            });
        }

        var group = await _groupRepository.GetGroupWithNameAsync(addToGroupDto.GroupName);
        if (group == null)
        {
            return Unauthorized(new ResponseDto
            {
                Status = ResponseStatus.Error,
                Message = "Group does not exist",
            });
        }
        await _userRepository.AddUserToGroup(user, group);
        _userRepository.Update(user);
        var result = await _userRepository.SaveAllAsync();
        if (!result)
        {
            return BadRequest(new ResponseDto
            {
                Status = ResponseStatus.Error,
                Message = $"Group {group.Name} wasn't added to User {user.UserName}",
            });
        }

        return Ok();
    }

    private async Task<bool> UserExists(string username)
    {
        var result = await _userRepository.GetUserByUsernameAsync(username);
        return result != null;
    }
}
