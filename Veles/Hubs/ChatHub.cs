using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Hubs;

public class ChatHub : Hub
{
    private readonly IChatRepository _chatRepository;
    private readonly IGroupRepository _groupRepository;

    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public ChatHub(IChatRepository chatRepository, IGroupRepository groupRepository, IUserRepository userRepository,
        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// On connecting to hub add connecting user to groupHubs he is member
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public override async Task OnConnectedAsync()
    {
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
    /// <summary>
    /// Starts OnDisconnect actions
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await RemoveFromMessageGroup(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Sends message to other users in group
    /// </summary>
    /// <param name="createMessageDto"></param>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    [Authorize]
    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _userRepository.GetUserByIdAsync(Context.User!.GetUserId());
        if (sender == null)
        {
            throw new HubException("User doesn't exist");
        }

        var connectionGroup = await _groupRepository.GetGroupWithNameAsync(createMessageDto.GroupName);
        if (connectionGroup == null)
        {
            throw new HubException("Group doesn't exist");
        }

        var message = new Message
        {
            User = sender,
            Group = connectionGroup,
            CreatedDate = createMessageDto.Created,
            Text = createMessageDto.Content
        };
        await _chatRepository.AddMessage(message);

        var result = await _chatRepository.SaveAllAsync();
        if (!result)
        {
            throw new HubException("Message wasn't saved");
        }

        await Clients.Group(connectionGroup.Name).SendAsync("NewMessage", _mapper.Map<NewMessageDto>(message));
    }
    /// <summary>
    /// Adding connected user to hubs groups
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    private async Task AddToMessageGroup(Group group)
    {
        var connection = new Connection(Context.ConnectionId, group);

        await _groupRepository.AddConnectionAsync(connection);

        if (await _groupRepository.SaveAllAsync())
        {
            return;
        }

        throw new HubException("Failed to join group");
    }

    /// <summary>
    /// Removing connections from database and hubs based on connectionId
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    private async Task RemoveFromMessageGroup(string connectionString)
    {
        var connections = await _groupRepository.GetConnectionsAsync(connectionString);
        if (connections == null)
        {
            throw new HubException("No connections to remove");
        }

        // Remove connection of user from connected groups from SignalR groups
        foreach (var connection in connections)
        {
            await Groups.RemoveFromGroupAsync(connectionString, connection.Group.Name);
        }

        _groupRepository.RemoveConnections(connections);

        if (await _groupRepository.SaveAllAsync())
        {
            return;
        }

        throw new HubException("Failed to remove from group");
    }
}
