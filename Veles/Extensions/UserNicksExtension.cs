using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Extensions;

public static class UserNicksExtension
{
    public static List<GroupNick> GetUserNicks(this User user)
    {
        var userGroupSelect = user.UserGroups
            .Where(ug => ug.UserId == user.Id)
            .Select(ug => new {ug.GroupId, ug.UserGroupNick});

        return userGroupSelect.Select(pair => new GroupNick(pair.GroupId, pair.UserGroupNick)).ToList();
    }
}
