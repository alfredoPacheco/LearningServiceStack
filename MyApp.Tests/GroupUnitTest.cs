using MyApp.Interface;
using MyApp.Logic;
using MyApp.Model;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace MyApp.Tests
{
    public class GroupUnitTest
    {
        private readonly ServiceStackHost appHost;

        public GroupUnitTest()
        {
            appHost = new BasicAppHost()
            {
                ConfigureContainer = container =>
                {
                    var dbFactory = new OrmLiteConnectionFactory(
                        "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                        SqliteDialect.Provider);

                    container.Register<IDbConnectionFactory>(dbFactory);
                    container.RegisterAutoWiredAs<BasicOrmMessageRepository, IMessageRepository>();
                    container.RegisterAutoWired<MessengerService>();
                }
            }.Init();
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
        public void ShouldCreateNewGroupsOnRequest()
        {
            var service = appHost.Container.Resolve<MessengerService>();

            var response = service.Post(new Group {
                GroupName = "NewGroup",
                Creator = "Me"
            });

            Assert.That(response.Name.Equals("NewGroup"));
        }
    }
}
