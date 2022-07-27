﻿using VelesAPI.DbContext;
using VelesLibrary.DTOs;
using VelesLibrary.DbModels;

namespace VelesAPI
{
    //Adding example data to database
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ChatDataContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            AddUser(context);
        }

        private static void AddUser(ChatDataContext context)
        {
            var user = context.Users.FirstOrDefault();//checks if there is any data
            if (user != null) return;

            var group1 = new Group { Name = "nowaki" };
            var user1 = new User
            {
                UserName = "Adam3",
                Email = "a@da.m",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.adam.ma/avatar",

                Groups = new List<Group> { group1 }
            };

            var user2 = new User
            {
                UserName = "Karol",
                Email = "karol@k.pl",
                Password = "1234",
                PasswordHash = BitConverter.GetBytes(1234),
                PasswordSalt = BitConverter.GetBytes(1234),
                Avatar = "https://www.karol.ma/avatar",

                Groups = new List<Group> { group1 }
            };

            context.Users.Add(user2);

            context.Messages.Add(new Message()
            {
                Text = "Initial",
                CreatedDate = DateTime.UtcNow,
                User = user1,
                Group = group1,
            });

            context.SaveChanges();
        }
    }
}