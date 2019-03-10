using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using Chapter1.ServiceInterface;
using Chapter1.ServiceModel;
using System.Threading.Tasks;
using System.Linq;
using System;
using Chapter1.ServiceLogic;
using ServiceStack.OrmLite;
using ServiceStack.Data;

namespace Chapter1.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost() {
                ConfigureContainer = container =>
                {
                    var dbFactory = new OrmLiteConnectionFactory(
                        "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                        SqliteDialect.Provider);

                    container.Register<IDbConnectionFactory>(dbFactory);

                    container.RegisterAutoWiredAs<BasicOrmMessageRepository,
                        IMessageRepository>();

                    container.RegisterAutoWired<MessengerService>();
                }
            }.Init();
            appHost.Container.AddTransient<GreetingServices>();
            appHost.Container.AddTransient<MessengerService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [SetUp]
        public void SetUp()
        {
            using (var db = appHost.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.DropAndCreateTable<Message>();
            }
        }

        [Test]
        public void Can_call_MyServices()
        {
            var service = appHost.Container.Resolve<GreetingServices>();

            var response = (GreetingResponse)service.Get(new Greeting { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void ShouldBeThreadSafe()
        {
            var service = appHost.Container.Resolve<MessengerService>();
            const string testGroupName = "Main";
            const int iterations = 200;

            Parallel.ForEach(
                Enumerable.Range(1, iterations),
                iteration =>
                {
                    service.Post(new Message
                    {
                        Body = "Post {0}".Fmt(iteration),
                        Sender = "Sender",
                        GroupName = testGroupName
                    });
                    service.Get(new Search
                    {
                        Group = testGroupName,
                        Query = "Post"
                    });
                });
            var testGroup = service.Get(new Group
            {
                GroupName = testGroupName
            });

            var randomSearchString = "Post {0}"
                .Fmt(new Random().Next(1, iterations));

            Assert.AreEqual(1, testGroup.Messages
                .Count(m => m.Body.Equals(randomSearchString)));
        }
    }
}
