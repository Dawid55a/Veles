using VelesAPI.DbContext;
using VelesAPI.DbModels;

namespace VelesAPI
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();  
            using var context = scope.ServiceProvider.GetRequiredService<ChatDataContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            AddUser(context);
            //var a = context.
        }

        private static void AddUser(ChatDataContext context)
        {
            var user = context.Users.FirstOrDefault();
            if (user != null) return;

            var group1 = new Group { Name = "nowaki" };
            var user1 = new User
            {
                Name = "Adam3",
                Email = "a@da.m",
                Password = "1234",
                Avatar = "https://www.adam.ma/avatar",

                Groups = new List<Group> { group1 }
            };
            

            context.Messages.Add(new Message()
            {
                Text = "Initial",
                CreatedDate = DateTime.UtcNow,
                User = user1,
                Group = group1,
            }) ;

            context.SaveChanges();
        }
    }
}
