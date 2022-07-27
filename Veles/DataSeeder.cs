using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesLibrary.DTOs;
using VelesLibrary.DbModels;

namespace VelesAPI
{
    //Adding example data to database
    public static class DataSeeder
    {
        public static async Task Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            try
            {
                await using var context = scope.ServiceProvider.GetRequiredService<ChatDataContext>();
                await context.Database.MigrateAsync();
                await SeedUsers(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private static async Task SeedUsers(ChatDataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var groupsKarolsAdam = new List<Group>
            {
                new Group()
                {
                    Name = "Karols"
                },
                new Group()
                {
                    Name = "Adams"
                }
            };

            var groupsKarols = new List<Group>
            {
                new Group()
                {
                    Name = "Karols"
                }
            };

            var users = new List<User>
            {
                new User()
                {
                    UserName = "Karol",
                    Email = "a@da.m",
                    Password = "1234",
                    PasswordHash = BitConverter.GetBytes(1234),
                    PasswordSalt = BitConverter.GetBytes(1234),
                    Avatar = "https://www.karol.ma/avatar",
                    Groups = groupsKarolsAdam
                },
                new User()
                {
                    UserName = "Adam",
                    Email = "a@da.m",
                    Password = "1234",
                    PasswordHash = BitConverter.GetBytes(1234),
                    PasswordSalt = BitConverter.GetBytes(1234),
                    Avatar = "https://www.adam.ma/avatar",
                    Groups = groupsKarolsAdam
                },
                new User()
                {
                    UserName = "Kamil",
                    Email = "a@da.m",
                    Password = "1234",
                    PasswordHash = BitConverter.GetBytes(1234),
                    PasswordSalt = BitConverter.GetBytes(1234),
                    Avatar = null,
                    Groups =  new List<Group>
                    {
                        new Group()
                        {
                            Name = "Kamils"
                        }
                    }
                    }
            };




            var messages = new List<Message>()
            {
                new Message()
                {
                    Text = "Witamy!"
                },
                new Message()
                {
                    Text = "Hallo!"
                },
                new Message()
                {
                    Text = "Witamy u karola"
                },
                new Message()
                {
                    Text = "Witamy u Adama, lol"
                },
                new Message()
                {
                    Text = "Hej, karol, jak się masz"
                },
                new Message()
                {
                    Text = "Test polskich znaków, ę ł ą ć ż ź"
                }
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
}
