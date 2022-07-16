using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbModels;
using VelesAPI.Interfaces;

namespace VelesAPI.DbContext
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatDataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ChatDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
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
            return await _context.Users
                .SingleOrDefaultAsync(x => x.Name == username);
        }

        public async Task<Group> GetGroupByNameTask(string groupName)
        {
            return await _context.Groups.FindAsync(groupName);
        }
    }
}
