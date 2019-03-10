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
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });

            //Plugins.Add(new ValidationFeature());

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[]{
                    new BasicAuthProvider()
                }));

            Plugins.Add(new RegistrationFeature());

            Plugins.Add(new CorsFeature());

            container.RegisterAutoWired<MyAppSettings>();
            var appSettings = container.Resolve<MyAppSettings>();

            var dbFactory = new OrmLiteConnectionFactory(
                appSettings.Get("sqlLiteConnectionString", "").MapHostAbsolutePath(),
                SqliteDialect.Provider);
            
            container.Register<IDbConnectionFactory>(dbFactory);

            //container.RegisterAutoWiredAs<BasicOrmMessageRepository, IMessageRepository>();

            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<Place>();
            }

        }
    }
}
