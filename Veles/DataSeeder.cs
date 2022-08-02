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
                Groups = groupsKarolsAdam
            },
            new()
            {
                UserName = "Adam",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.adam.ma/avatar",
                Groups = groupsKarolsAdam
            },
            new()
            {
                UserName = "Kamil",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = null,
                Groups = new List<Group> {new() {Name = "Kamils"}}
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
    }
}
