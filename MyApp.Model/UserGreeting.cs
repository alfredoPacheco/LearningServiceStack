using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Model
{
    public class UserGreeting
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Greeting { get; set; }

        [References(typeof(UserLanguage))]
        public int UserLanguageId { get; set; }

        [Reference]
        public UserLanguage Language { get; set; }
    }

    public class UserLanguage
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Language { get; set; }
    }


    [Route("/greetings/{Id}/sayto/{Name}")]
    public class GetUserGreeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetUserGreetingResponse
    {
        public string Result { get; set; }
    }
}
