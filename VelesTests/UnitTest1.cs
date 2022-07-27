using System.Dynamic;
using AutoMapper;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using SignalR_UnitTestingSupport.Hubs;
using VelesAPI.DbContext;
using VelesAPI.Hubs;
using VelesAPI.Interfaces;

namespace VelesTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

    }

    /*public class ExampleHubTests : HubUnitTestsWithEF<ChatDataContext>
    {
        [SetUp]
        public override void SetUp()
        {
            //https://github.com/dotnet/aspnetcore/tree/main/src/SignalR/samples/JwtClientSample
        }

        [Test]
        public void MockSendMessage()
        {
   

            bool sendCalled = false;
            var hub = new ChatHub();
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.broadcastMessage = new Action<string, string>((name, message) => {
                sendCalled = true;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            hub.Send("TestUser", "TestMessage");
            Assert.True(sendCalled);
        }

        public void MockSendMessage2()
        {
            
        }


    }*/
}