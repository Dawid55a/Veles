using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VelesLibrary.DTOs;
using VelesLibrary.DbModels;
using VelesAPI.Interfaces;

namespace VelesAPI.DbContext
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDataContext _context;
        private readonly IMapper _mapper;

        public ChatRepository(ChatDataContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChangesAsync();
        }

        public void RemoveMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id) ?? throw new InvalidOperationException();
        }

        public async Task<Group> GetGroupForUserId(int id)
        {
            // TODO: Returns only first group
            var user = await _context.Users.FindAsync(id);
            var Groups = await (from g in _context.Groups where g.Users.Any(u => u.Id == id) select g).ToListAsync();
            return Groups[0];
        }

        public async Task<IEnumerable<Group>> GetGroupsForUserTask(User user)
        {
            return await _context.Groups.Where(g => g.Users.Any(u => u.Id == user.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsForUserNameTask(string username)
        {
            var user = await _context.Users.FindAsync(username) ?? throw new InvalidOperationException();
            return await _context.Groups.Where(g => g.Users.Any(u => u.Id == user.Id)).ToListAsync();
        }


        public async Task<Group> GetGroup(int id)
        {
            return await _context.Groups.FindAsync(id) ?? throw new InvalidOperationException();
        }

        public async Task<User> GetUser(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == name) ?? throw new InvalidOperationException();
        }

        public async Task<Group> GetGroup(string name)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Name == name) ?? throw new InvalidOperationException();
        }

        public Task<IEnumerable<Message>> GetMessageThreadForUserAndGroup(User user, Group group)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEnumerable<Message>>> MessageThreadsFromUsersGroups(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetMessageThreadTask(Group g)
        {
            var messages = await _context.Messages
                .Where(m => m.Group.Id == g.Id)
                .ToListAsync();
            return messages;
        }

        public async Task<Group> GetGroupForConnection(string connection)
        {
            var query = await (from g in _context.Groups
                join c in _context.Connections on g equals c.Group
                where c.ConnectionId == connection
                select c.Group).ToListAsync();
            if (query.Count > 1)
            {
                throw new ArgumentOutOfRangeException("Returned more than one group! Should be one");
            }
            return query[0];
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
