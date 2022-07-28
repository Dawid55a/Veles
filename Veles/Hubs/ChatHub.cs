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

    private readonly IMapper _mapper;

    //private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public ChatHub(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
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
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
        }

        var allMessages = new List<IEnumerable<Message>>();

        foreach (var group in groups)
        {
            var messages = await _chatRepository.GetMessageThreadAsync(group);
            allMessages.Add(messages);
        }

        await Clients.Caller.SendAsync("ReceiveMessageThreadsFromUsersGroups", allMessages);*/
        var username = Context.User!.GetUsername();
        var groups = await _chatRepository.GetGroupsForUserNameAsync(username);
        foreach (var group in groups)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
            AddToGroup(group);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group = await RemoveFromMessageGroup();
        await base.OnDisconnectedAsync(exception);
    }


    // NewMessage

    // ReceiveMessagesFromGroup
    /// <summary>
    ///     Get all messagesDto's for specified group
    /// </summary>
    /// <param name="groupName">Name of the group</param>
    /// <returns><see cref="T:System.Collections.Generic.List`1" /> containing all messages</returns>
    [Authorize]
    public async Task RequestMessagesFromGroup(string groupName)
    {
        //var user = await _userRepository.GetUserByUsernameAsync(Context.User.GetUsername());
        var user = await _userRepository.GetUserByUsernameAsync("Karol");
        // Check if user can receive messages from group
        var groups = await _chatRepository.GetGroupsForUserNameAsync(user.UserName);
        var res =
            from g in groups
            where g.Users.Contains(user)
            select g;

        if (res.Any())
        {
            var messages = await _chatRepository.GetMessageThreadAsync(res.First(g => g.Name == groupName));
            await Clients.Caller.SendAsync("ReceiveMessagesFromGroup", _mapper.Map<IEnumerable<MessageDto>>(messages));
            return;
        }

        throw new NullReferenceException("User is not in any group");
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _userRepository.GetUserByUsernameAsync(createMessageDto.Sender);
        //var sender = Context.User.GetUsername();
        var connectionGroup = await _chatRepository.GetGroupForUserIdAsync(sender.Id);

        var message = new Message {User = sender, Group = connectionGroup, Text = createMessageDto.Content};

        _chatRepository.AddMessage(message);

        await Clients.Group(connectionGroup.Name).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
    }

    /*public async Task ReciveMessages()
    {

        var sender = Context.User.GetUsername();
        //await Clients.Caller

    }*/

    private async void AddToGroup(Group group)
    {
        //var group = await _userRepository.GetGroupByNameAsync(groupName);
        var connection = new Connection(Context.ConnectionId, group);


        group.Connections.Add(connection);

        if (await _userRepository.SaveAllAsync())
        {
            return;
        }

        throw new HubException("Failed to join group");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await _chatRepository.GetGroupForConnectionAsync(Context.ConnectionId);

        var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        _chatRepository.RemoveConnection(connection);

        if (await _chatRepository.SaveAllAsync())
        {
            return group;
        }

        throw new HubException("Failed to remove from group");
    }
}
