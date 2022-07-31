using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Hubs;

public class ChatHub : Hub
{
    private readonly ChatDataContext _dataContext;
    private readonly IChatRepository _chatRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    //private readonly IMapper _mapper;

    public ChatHub(ChatDataContext dataContext, IChatRepository chatRepository, IGroupRepository groupRepository, IUserRepository userRepository, IMapper mapper)
    {
        _dataContext = dataContext;
        _chatRepository = chatRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        /*var username = Context.User!.GetUsername();
        
        // TODO: Implement adding users Groups to Groups in Hubs Groups
        var groups = await _chatRepository.GetGroupsForUserNameAsync(username);

        foreach (var group in groups)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
        }

        var allMessages = new List<IEnumerable<Message>>();

        foreach (var group in groups)
        {
            var messages = await _chatRepository.GetMessageThreadAsync(group);
            allMessages.Add(messages);
        }

        await Clients.Caller.SendAsync("ReceiveMessageThreadsFromUsersGroups", allMessages);
        */
        var userId = Context.User!.GetUserId();
        var groups = await _chatRepository.GetGroupsForUserIdIncludingConnectionsAsync(userId);
        foreach (var group in groups)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
        }

        foreach (var group in groups)
        {
            await AddToMessageGroup(group);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await RemoveFromMessageGroup(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    [Authorize]
    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _userRepository.GetUserByIdAsync(Context.User.GetUserId());
        if (sender == null)
        {
            throw new HubException("User doesn't exist");
        }

        var connectionGroup = await _groupRepository.GetGroupWithNameAsync(createMessageDto.GroupName);
        if (connectionGroup == null)
        {
            throw new HubException("Group doesn't exist");
        }

        var message = new Message {User = sender, Group = connectionGroup, CreatedDate = createMessageDto.Created, Text = createMessageDto.Content};
        await _chatRepository.AddMessage(message);

        var result = await _chatRepository.SaveAllAsync();
        if (!result)
        {
            throw new HubException("Message wasn't saved");
        }

        await Clients.Group(connectionGroup.Name).SendAsync("NewMessage", _mapper.Map<NewMessageDto>(message));
    }

    private async Task AddToMessageGroup(Group group)
    {

        //var group = await _userRepository.GetGroupByNameAsync(groupName);
        var connection = new Connection(Context.ConnectionId, group);
        await _dataContext.Connections.AddAsync(connection);
        //_groupRepository.UpdateGroup(group);
        
        //_groupRepository.AddConnection(connection);
        
        if (await _dataContext.SaveChangesAsync() > 0)
        {
            return;
        }

        throw new HubException("Failed to join group");
    }

    private async Task RemoveFromMessageGroup(string connectionString)
    {
        var groupr = await _dataContext.Connections
            .Include(c => c.Group)
            .Where(c => c.ConnectionString == connectionString)
            .ToListAsync();

        // Remove connection of user from connected groups from SignalR groups
        foreach (var connection in groupr)
        {
            await Groups.RemoveFromGroupAsync(connectionString, connection.Group.Name);
        }
        //_chatRepository.RemoveConnection(connection);
        _dataContext.Connections.RemoveRange(groupr);
        
        if (await _dataContext.SaveChangesAsync() > 0)
        {
            return;
        }

        throw new HubException("Failed to remove from group");
    }
}
