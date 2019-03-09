using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using Chapter1.ServiceInterface;
using Chapter1.ServiceModel;

namespace Chapter1.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<GreetingServices>();
            appHost.Container.AddTransient<MessengerService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Can_call_MyServices()
        {
            var service = appHost.Container.Resolve<GreetingServices>();

            var response = (GreetingResponse)service.Get(new Greeting { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}
