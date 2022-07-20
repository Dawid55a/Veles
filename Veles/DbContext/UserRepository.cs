using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbModels;
using VelesAPI.Interfaces;

namespace VelesAPI.DbContext
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatDataContext _context;

        public UserRepository(ChatDataContext context)
        {
            _context = context;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async void AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context!.Users!.SingleOrDefaultAsync(x => x.UserName == username) ;
        }

        public async Task<Group> GetGroupByNameTask(string groupName)
        {
            return await _context.Groups.FindAsync(groupName);
        }
    }
}
