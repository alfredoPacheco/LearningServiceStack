using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using MyApp.Interface;
using MyApp.Model;
using ServiceStack.Validation;
using MyApp.Logic;
using System.Collections.Generic;
using ServiceStack.Caching;

namespace MyApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("MyApp", typeof(PlaceService).Assembly) { }


        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            #region Metadata
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });
            #endregion

            #region Plugins
            Plugins.Add(new ValidationFeature());
            container.RegisterValidators(
                typeof(CreatePlaceValidator).Assembly);

            Plugins.Add(new CorsFeature());

            Plugins.Add(new PostmanFeature());
            #endregion

            #region App
            container.RegisterAutoWiredAs<BasicOrmMessageRepository, IMessageRepository>();
            container.RegisterAutoWired<MyAppSettings>();
            var appSettings = container.Resolve<MyAppSettings>();
            #endregion

            #region Database
            var dbFactory = new OrmLiteConnectionFactory(
                appSettings.Get("sqlLiteConnectionString", "").MapHostAbsolutePath(),
                SqliteDialect.Provider);

            container.Register<IDbConnectionFactory>(dbFactory);

            using (var db = dbFactory.OpenDbConnection())
            {
                //db.DropAndCreateTable<UserGreeting>();
                db.DropAndCreateTable<UserLanguage>();
                db.DropAndCreateTable<GreetingUsage>();
            }

            using (var db = dbFactory.OpenDbConnection())
            {
                db.Insert(new UserLanguage { Language = "English" });
                //db.Insert(new UserGreeting
                //{
                //    Greeting = "Hello, {0}",
                //    UserLanguageId = 1
                //});
                //db.Insert(new UserGreeting
                //{
                //    Greeting = "G'day, {0}"
                //});
                //db.Insert(new UserGreeting
                //{
                //    Greeting = "Howdy, {0}!"
                //});
            }
            #endregion

            #region Auth
            var authProviders = new List<IAuthProvider>();
            authProviders.Add(new CredentialsAuthProvider());

            var authFeature = new AuthFeature(SessionFactory, authProviders.ToArray());
            Plugins.Add(authFeature);

            var authRepo = new OrmLiteAuthRepository(dbFactory);
            container.Register<IUserAuthRepository>(authRepo);
            container.Register<ICacheClient>(new MemoryCacheClient());
            authRepo.DropAndReCreateTables();
            authRepo.InitSchema();

            Plugins.Add(new RegistrationFeature());
            //CreateUsers(userRep);
            #endregion
        }

        private IAuthSession SessionFactory()
        {
            return new AuthUserSession();
        }
    }
}
