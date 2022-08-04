using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesLibrary.DbModels;

namespace VelesAPI;

/// <summary>
///     Adding example data to database
/// </summary>
public static class DataSeeder
{
    /// <summary>
    ///     Run seed on host's DbContext if there is no data in database
    /// </summary>
    /// <param name="host"></param>
    /// <returns>Task</returns>
    public static async Task Seed(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        try
        {
            await using var context = scope.ServiceProvider.GetRequiredService<ChatDataContext>();
            await context.Database.MigrateAsync();
            await SeedData(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    ///     Creating data and applying it to database
    /// </summary>
    /// <param name="context">ChatDataContext</param>
    /// <returns>Task</returns>
    private static async Task SeedData(ChatDataContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        /*var users = new List<User>
        {
            new()
            {
                UserName = "Karol",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.karol.ma/avatar",
                UserGroups = null
            },
            new()
            {
                UserName = "Adam",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.adam.ma/avatar",
                UserGroups = new List<UserGroup>()
            },
            new()
            {
                UserName = "Kamil",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = null,
                UserGroups = new List<UserGroup>()
            }
        };

        var groupsKarolsAdam = new List<Group> { new() { Name = "Karols", UserGroups = new List<UserGroup>()}, new() { Name = "Adams", UserGroups = new List<UserGroup>()} };
        var groupsKamils = new List<Group> {new() {Name = "Kamils"}};

        foreach (var user in users)
        {
            foreach (var group in groupsKarolsAdam)
            {
                var ug = new UserGroup() {User = user, Group = group, UserGroupNick = user.UserName};
                user.UserGroups.Add(ug);
                group.UserGroups.Add(ug);
            }
        }*/


        var user1 = new User()
        {
            UserName = "Karol",
            Email = "a@da.m",
            Password = "1234",
            PasswordHash = BitConverter.GetBytes(1234),
            PasswordSalt = BitConverter.GetBytes(1234),
            Avatar = "https://www.karol.ma/avatar",
            UserGroups = new List<UserGroup>()
        };
        
        using var hmac1 = new HMACSHA512();

        user1.UserName = user1.UserName.ToLower();
        user1.PasswordSalt = hmac1.Key;
        user1.PasswordHash = hmac1.ComputeHash(Encoding.UTF8.GetBytes("1234"));
        var user2 = new User()
        {
            UserName = "Adam",
            Email = "a@da.m",
            Password = "1234",
            PasswordHash = BitConverter.GetBytes(1234),
            PasswordSalt = BitConverter.GetBytes(1234),
            Avatar = "https://www.karol.ma/avatar",
            UserGroups = new List<UserGroup>()
        };
        using var hmac2 = new HMACSHA512();

        user2.UserName = user2.UserName.ToLower();
        user2.PasswordSalt = hmac2.Key;
        user2.PasswordHash = hmac2.ComputeHash(Encoding.UTF8.GetBytes("1234"));
        var group1 = new Group() {Name = "Users1", UserGroups = new List<UserGroup>()};
        var group2 = new Group() { Name = "Users2", UserGroups = new List<UserGroup>()};

        var user1group1 = new UserGroup()
        {
            User = user1,
            UserId = user1.Id,
            Group = group1,
            GroupId = group1.Id,
            UserGroupNick = user1.UserName
        };
        var user1group2 = new UserGroup()
        {
            User = user1,
            UserId = user1.Id,
            Group = group2,
            GroupId = group2.Id,
            UserGroupNick = user1.UserName
        };
        var user2group2 = new UserGroup()
        {
            User = user2,
            UserId = user2.Id,
            Group = group2,
            GroupId = group2.Id,
            UserGroupNick = user2.UserName
        };

        var messages = new List<Message>
        {
            new() {Text = "Witamy!"},
            new() {Text = "Hallo!"},
            new() {Text = "Witamy u karola"},
            new() {Text = "Witamy u Adama, lol"},
            new() {Text = "Hej, karol, jak się masz"},
            new() {Text = "Test polskich znaków, ę ł ą ć ż ź"}
        };

        messages[0].Group = group1;
        messages[1].Group = group2;
        messages[2].Group = group1;
        messages[3].Group = group2;
        messages[4].Group = group2;
        messages[5].Group = group2;

        messages[0].User = user1;
        messages[1].User = user1;
        messages[2].User = user1;
        messages[3].User = user1;
        messages[4].User = user2;
        messages[5].User = user2;


        user1.UserGroups.Add(user1group1);
        user1.UserGroups.Add(user1group2);
        user2.UserGroups.Add(user2group2);
        group1.UserGroups.Add(user1group1);
        group2.UserGroups.Add(user1group2);
        group2.UserGroups.Add(user2group2);

        await context.AddAsync(user1);
        await context.AddAsync(user2);
        await context.AddAsync(group1);
        await context.AddAsync(group2);
        foreach (var message in messages)
        {
            await context.AddAsync(message);
        }

        await context.SaveChangesAsync();
        /*foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));
        }

        await context.AddAsync(users);
        await context.AddAsync(groupsKarolsAdam);
        await context.SaveChangesAsync();*/

        /*if (await context.Users.AnyAsync())
        {
            return;
        }

        var groupsKarolsAdam = new List<Group> {new() {Name = "Karols"}, new() {Name = "Adams"}};
        var users = new List<User>
        {
            new()
            {
                UserName = "Karol",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.karol.ma/avatar",
                UserGroups = groupsKarolsAdam
            },
            new()
            {
                UserName = "Adam",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.adam.ma/avatar",
                UserGroups = groupsKarolsAdam
            },
            new()
            {
                UserName = "Kamil",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = null,
                UserGroups = new List<Group> {new() {Name = "Kamils"}}
            }
        };

        var messages = new List<Message>
        {
            new() {Text = "Witamy!"},
            new() {Text = "Hallo!"},
            new() {Text = "Witamy u karola"},
            new() {Text = "Witamy u Adama, lol"},
            new() {Text = "Hej, karol, jak się masz"},
            new() {Text = "Test polskich znaków, ę ł ą ć ż ź"}
        };

        messages[0].Group = groupsKarolsAdam[0];
        messages[1].Group = groupsKarolsAdam[1];
        messages[2].Group = groupsKarolsAdam[0];
        messages[3].Group = groupsKarolsAdam[1];
        messages[4].Group = groupsKarolsAdam[0];
        messages[5].Group = groupsKarolsAdam[0];

        messages[0].User = users[0];
        messages[1].User = users[1];
        messages[2].User = users[0];
        messages[3].User = users[1];
        messages[4].User = users[0];
        messages[5].User = users[1];

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));

            await context.Users.AddAsync(user);
        }

        foreach (var group in groupsKarolsAdam)
        {
            await context.Groups.AddAsync(group);
        }

        foreach (var message in messages)
        {
            await context.Messages.AddAsync(message);
        }

        await context.SaveChangesAsync();
        */
    }
}
