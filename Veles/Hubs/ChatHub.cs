using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VelesAPI.DbModels;
using VelesAPI.DTOs;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;

namespace VelesAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;
        //private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public ChatHub(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var username = Context.User!.GetUsername();
            
            // TODO: Implement adding users Groups to Groups in Hubs Groups
            var groups = await _chatRepository.GetGroupsForUserNameTask(username);

            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
            }

            var allMessages = new List<IEnumerable<Message>>();

            foreach (var group in groups)
            {
                var messages = await _chatRepository.GetMessageThreadTask(group);
                allMessages.Add(messages);
            }

            await Clients.Caller.SendAsync("ReceiveMessageThreadsFromUsersGroups", allMessages);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _userRepository.GetUserByUsernameAsync(createMessageDto.Sender);
            //var sender = Context.User.GetUsername();
            var connectionGroup = await _chatRepository.GetGroupForUserId(sender.Id);
            
            var message = new Message
            {
                User = sender,
                Group = connectionGroup,
                Text = createMessageDto.Content
                
            };
            
            _chatRepository.AddMessage(message);

            //await Clients.Group(connectionGroup.UserName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            
        }

        public async Task ReciveMessages()
        {

            var sender = Context.User.GetUsername();
            //await Clients.Caller

        }


    }
}
