using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public ChatHub(IChatRepository chatRepository, IMapper mapper, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.GetUserId();
            // TODO: Implement adding users Groups to Groups in Hubs Groups
            var connectionGroup = await _chatRepository.GetGroupForUserId(userId);

            await Groups.AddToGroupAsync(Context.ConnectionId, connectionGroup.Name);

            var messages = await _chatRepository.GetMessageThreadTask(connectionGroup);

            await Clients.Group(connectionGroup.Name).SendAsync("ReceiveMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var userId = Context.User.GetUserId();
            var connectionGroup = await _chatRepository.GetGroupForUserId(userId);
            var sender = await _userRepository.GetUserByIdAsync(userId);

            var message = new Message
            {
                User = sender,
                Group = connectionGroup,
                Text = createMessageDto.Content
                
            };

            _chatRepository.AddMessage(message);

            await Clients.Group(connectionGroup.Name).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            
        }


    }
}
