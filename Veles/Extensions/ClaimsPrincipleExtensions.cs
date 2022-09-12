using System.Security.Claims;

namespace VelesAPI.Extensions;

public static class ClaimsPrincipleExtensions
{
    /// <summary>
    /// Get user name from token
    /// </summary>
    /// <param name="user"></param>
    /// <returns>userName</returns>
    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
    /// <summary>
    /// Get user id from token
    /// </summary>
    /// <param name="user"></param>
    /// <returns>userId</returns>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
