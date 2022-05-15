using System.Linq;
using VelesServer.DataModels;
using VelesServer.Models;

namespace VelesServer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(VelesContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            var users = new User[]
            {
                new User {Email = "test@test1.pl", ID = 1, UserName = "Tom"},
                new User {Email = "elisese@test2.pl", ID = 2, UserName = "Elise"},
                new User {Email = "emi@test3.pl", ID = 3, UserName = "Emanuel"}
            };


            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}