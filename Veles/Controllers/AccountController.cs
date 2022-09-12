using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class AccountController : BaseApiController
{
    private readonly IChatRepository _chatRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository, ITokenService tokenService,
        IGroupRepository groupRepository, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _groupRepository = groupRepository;
        _chatRepository = chatRepository;
    }

    /// <summary>
    /// Registering user to application based on registerDto, validating data and returning proper response.
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns>ActionResult</returns>
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

        switch (registerDto.Password.Length)
        {
            case > 20:
                return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "Password too long"});
            case < 6:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Password too short" });
        }

        switch (registerDto.UserName.Length)
        {
            case > 20:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Username too long" });
            case < 3:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Username too short" });
        }

        if (!IsValidEmail(registerDto.Email))
        {
            return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Incorrect email"});
        }

        var user = new User
        {
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
            Email = registerDto.Email.ToLower()
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

    /// <summary>
    /// Logging to application with loginDto, checking password 
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns>Token on success otherwise response</returns>
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.UserName);

        if (user == null)
        {
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "User does not exist"});
        }

        if (user.Removed)
        {
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "Account is removed"});
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

    /// <summary>
    /// Adding user authorized with token to message group specified in AddToGroupDto
    /// </summary>
    /// <param name="addToGroupDto"></param>
    /// <returns>ActionResult</returns>
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
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "User does not exist"});
        }


        var group = await _groupRepository.GetGroupWithNameAsync(addToGroupDto.GroupName);
        if (group == null)
        {
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "Group does not exist"});
        }

        await _userRepository.AddUserToGroup(user, group, Roles.Member);
        _userRepository.Update(user);
        var result = await _userRepository.SaveAllAsync();
        if (!result)
        {
            return BadRequest(new ResponseDto
            {
                Status = ResponseStatus.Error, Message = $"Group {group.Name} wasn't added to User {user.UserName}"
            });
        }

        return Ok();
    }
    /// <summary>
    /// Changing user password
    /// </summary>
    /// <param name="changePasswordDto"></param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("change_password")]
    public async Task<ActionResult<TokenDto>> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
        {
            Unauthorized(new ResponseDto
            {
                Status = ResponseStatus.Error, Message = "New password is identical to current password"
            });
        }
        switch (changePasswordDto.NewPassword.Length)
        {
            case > 20:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Password too long" });
            case < 6:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Password too short" });
        }
        var user = await _userRepository.GetUserByUsernameAsync(changePasswordDto.UserName);
        if (user == null)
        {
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "User does not exist"});
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(changePasswordDto.OldPassword));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "Invalid password"});
            }
        }

        using var hmacNew = new HMACSHA512();
        user.PasswordHash = hmacNew.ComputeHash(Encoding.UTF8.GetBytes(changePasswordDto.NewPassword));
        user.PasswordSalt = hmacNew.Key;

        _userRepository.Update(user);
        if (!await _userRepository.SaveAllAsync())
        {
            return Unauthorized(new ResponseDto
            {
                Status = ResponseStatus.Error, Message = "Error in changing password"
            });
        }

        return Ok(new TokenDto {UserName = user.UserName, Token = _tokenService.CreateToken(user)});
    }
    /// <summary>
    /// Removing account and every group he owns, changing nick name to removed in all group he is
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("remove_account")]
    public async Task<ActionResult> RemoveAccount()
    {
        var contextUserId = User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(contextUserId);
        if (user == null)
        {
            return Unauthorized(new ResponseDto {Status = ResponseStatus.Error, Message = "User does not exist"});
        }

        user.Removed = true;
        _userRepository.Update(user);

        var groups = await _chatRepository.GetGroupsForUserIdAsync(user.Id);
        if (groups == null)
        {
            return Ok();
        }

        foreach (var group in groups)
        {
            var role = await _userRepository.GetUserRoleInGroup(user.Id, group.Id);
            if (role!.Equals(Roles.Owner))
            {
                _groupRepository.RemoveGroup(group);
            }
        }

        foreach (var group in groups)
        {
            await _userRepository.ChangeNickInUserGroup(user.Id, group.Id, "Removed");
        }

        if (await _userRepository.SaveAllAsync())
        {
            return Ok(new ResponseDto {Status = ResponseStatus.Success, Message = "Account removed"});
        }

        return Unauthorized("Changes was not saved");
    }
    /// <summary>
    /// Check if user with specified user name exist asynchronously
    /// </summary>
    /// <param name="username"></param>
    /// <returns>True or False</returns>
    private async Task<bool> UserExists(string username)
    {
        var result = await _userRepository.GetUserByUsernameAsync(username);
        return result != null;
    }
    /// <summary>
    /// Helper method for validating email
    /// </summary>
    /// <param name="email">email string</param>
    /// <returns>True of False</returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
