using System.Linq;
using VelesServer.DataModels;


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
                new User {Id = 1, Email = "mail@mail.com", Nick = "Tom", Avatar = "asaasasas", Password = "1234"},
            };


            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}