using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using VelesAPI.DTOs;
using VelesAPI.Hubs;
using VelesAPI.Interfaces;

namespace VelesTests.Hubs
{
    [TestFixture]
    public class ChatHubTests
    {
        private MockRepository mockRepository;

        private Mock<IChatRepository> mockChatRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IMapper> mockMapper;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockChatRepository = this.mockRepository.Create<IChatRepository>();
            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
        }

        private ChatHub CreateChatHub()
        {
            return new ChatHub(
                this.mockChatRepository.Object,
                this.mockUserRepository.Object,
                this.mockMapper.Object);
        }

        /*[Test]
        public async Task OnConnectedAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var chatHub = this.CreateChatHub();

            // Act
            await chatHub.OnConnectedAsync();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task OnDisconnectedAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var chatHub = this.CreateChatHub();
            Exception? exception = null;

            // Act
            await chatHub.OnDisconnectedAsync(
                exception);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }*/

        [Test]
        public async Task RequestMessagesFromGroup_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var chatHub = this.CreateChatHub();
            string groupName = "nowaki";
            
            
            // Act
            await chatHub.RequestMessagesFromGroup(
                groupName);

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task SendMessage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var chatHub = this.CreateChatHub();
            CreateMessageDto createMessageDto = null;

            // Act
            await chatHub.SendMessage(
                createMessageDto);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
