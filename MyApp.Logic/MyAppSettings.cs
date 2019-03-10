using Microsoft.Extensions.Configuration;
using ServiceStack;
using System.Collections.Generic;

namespace MyApp
{
    public class MyAppSettings : NetCoreAppSettings
    {
        public MyAppSettings(IConfiguration configuration) : base(configuration)
        {
        }

        public ApplicationEnvironment Environment
        {
            get
            {
                return Get("Environment", ApplicationEnvironment.Dev);
            }
        }

        public EmailSettings EmailSettings
        {
            get
            {
                var settingsName = "EmailSettings_" + Environment;
                return Get(settingsName, (EmailSettings)null);
            }
        }

        public List<string> AdministratorEmails
        {
            get
            {
                return Get("AdminEmailAddresses", new List<string>());
            }
        }

        public enum ApplicationEnvironment
        {
            Dev,
            Test,
            Prod
        }

    }

    public class EmailSettings
    {
        public string SMPTUrl { get; set; }
        public int SMTPPort { get; set; }
    }
}
