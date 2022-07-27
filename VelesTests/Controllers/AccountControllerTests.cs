using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Controllers;
using VelesAPI.DTOs;
using VelesAPI.Interfaces;

namespace VelesTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IUserRepository> mockUserRepository;
        private Mock<ITokenService> mockTokenService;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockTokenService = this.mockRepository.Create<ITokenService>();
        }

        private AccountController CreateAccountController()
        {
            return new AccountController(
                this.mockUserRepository.Object,
                this.mockTokenService.Object);
        }

        [Test]
        public async Task Register_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var accountController = this.CreateAccountController();
            RegisterDto registerDto = new RegisterDto()
            {
                UserName= "dave2",
                Password = "password",
                Email = "email@email.com"
            };

            // Act
            var result = await accountController.Register(
                registerDto);

            // Assert
            Assert.IsInstanceOf<ActionResult<UserDto>>(result);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task Login_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var accountController = this.CreateAccountController();
            LoginDto loginDto = new LoginDto()
            {
                UserName = "dave",
                Password = "password"
            };

            // Act
            var result = await accountController.Login(
                loginDto);

            // Assert
            Assert.IsInstanceOf<ActionResult<UserDto>>(result);
            
            this.mockRepository.VerifyAll();
        }
    }
}
