using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbModels;
using VelesAPI.DTOs;
using VelesAPI.Interfaces;

namespace VelesAPI.DbContext
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDataContext _context;
        private readonly IMapper _mapper;

        public ChatRepository(ChatDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == user.Groups.ToList()[0].Id);
        }

        public async Task<Group> GetGroup(int id)
        {
            return await _context.Groups.FindAsync(id) ?? throw new InvalidOperationException();
        }

        public async Task<User> GetUser(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name) ?? throw new InvalidOperationException();
        }

        public async Task<Group> GetGroup(string name)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Name == name) ?? throw new InvalidOperationException();
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreadTask(Group g)
        {
            var messages = await _context.Messages
                .Where(m => m.Group.Id == g.Id)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return messages;
        }
    }
}
