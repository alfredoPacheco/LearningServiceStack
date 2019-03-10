using Chapter1.ServiceLogic;
using Chapter1.ServiceModel;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Chapter1.Tests
{
    [TestFixture]
    public class BasicOrmMessageRepositoryPerformanceTests
    {
        const int LargeMessageCount = 10000;
        ServiceStackHost appHost;

        public BasicOrmMessageRepositoryPerformanceTests()
        {
            appHost = new BasicAppHost
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                        "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                        SqliteDialect.Provider));
                    container
                    .RegisterAutoWired<BasicOrmMessageRepository>();
                }
            }.Init();
        }

        [SetUp]
        public void SetUp()
        {
            const string testDataFile =
                @"../../OrmMessageRepository_Performance_Test_Data.json";
            if (!File.Exists(testDataFile))
            {
                CreateTestFile(testDataFile, LargeMessageCount);
            }

            using (var db = appHost.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.DropAndCreateTable<Message>();
                var wholeList = File.ReadAllText(testDataFile)
                    .FromJson<List<Message>>();
                db.InsertAll(wholeList);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void ShouldHandleLargeMessageCountEffeciently()
        {
            var repo = appHost.Resolve<BasicOrmMessageRepository>();

            var randomSearchString = "Message {0}".Fmt(
                new Random().Next(1, LargeMessageCount));

            var searchTimer = new Stopwatch();
            searchTimer.Start();
            var testSearchRecords = repo.Where(
                message => message
                    .Body
                    .Contains(randomSearchString));
            searchTimer.Stop();

            Assert.AreEqual(randomSearchString,
                testSearchRecords.First().Body);
            Assert.Less(searchTimer.ElapsedMilliseconds, 150);
        }

        void CreateTestFile(string fileName, int testRecords)
        {
            Console.WriteLine("Creating test data...");
            var tmp = new List<Message>();
            foreach (int iteration in
            Enumerable.Range(1, testRecords))
            {
                tmp.Add(new Message
                {
                    Body = "Message {0}".Fmt(iteration),
                    GroupName = "performance test group",
                    Sender = "test sender"
                });
            }
            var wholeList = tmp.SerializeToString();
            File.WriteAllText(fileName, wholeList);
        }

    }
}
