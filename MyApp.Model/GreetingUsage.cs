using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Model
{
    public class GreetingUsage
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(UserGreeting))]
        public int UserGreetingId { get; set; }

        [Reference]
        public UserGreeting UserGreeting { get; set; }

        public UserLanguage Language { get; set; }
    }
}
