using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using VelesAPI.DbContext;
using VelesAPI.DbModels;

namespace VelesTests.DbContext
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private MockRepository mockRepository;

        private Mock<ChatDataContext> mockChatDataContext;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockChatDataContext = this.mockRepository.Create<ChatDataContext>();
        }

        private UserRepository CreateUserRepository()
        {
            return new UserRepository(
                this.mockChatDataContext.Object);
        }

        [Test]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            User user = null;

            // Act
            userRepository.Update(
                user);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void AddUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            User user = null;

            // Act
            userRepository.AddUser(
                user);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task SaveAllAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();

            // Act
            var result = await userRepository.SaveAllAsync();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetUsersAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();

            // Act
            var result = await userRepository.GetUsersAsync();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetUserByIdAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            int id = 0;

            // Act
            var result = await userRepository.GetUserByIdAsync(
                id);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetUserByUsernameAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string username = "Karol";

            // Act
            var result = await userRepository.GetUserByUsernameAsync(
                username);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGroupByNameTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string groupName = null;

            // Act
            var result = await userRepository.GetGroupByNameTask(
                groupName);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
