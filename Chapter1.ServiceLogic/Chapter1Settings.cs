﻿using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Configuration;
using System.Collections.Generic;

namespace Chapter1
{
    public class Chapter1Settings : NetCoreAppSettings
    {
        public Chapter1Settings(IConfiguration configuration) : base(configuration)
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
                return base.Get(settingsName, (EmailSettings)null);
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
