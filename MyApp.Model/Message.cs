using ServiceStack;
using System.Collections.Generic;

namespace MyApp.Model
{
    [Route("/message")]
    [Route("/message/{GroupName}")]
    public class Message
    {
        public string Body { get; set; }
        public string Sender { get; set; }
        public string GroupName { get; set; }
    }

    public class MessageResponse
    {
        public string Response { get; set; }
    }


    [Route("/message/search")]
    [Route("/message/search/{Group}")]
    public class Search
    {
        public string Group { get; set; }
        public string Query { get; set; }
    }

    public class SearchResponse
    {
        public List<Message> Messages { get; set; }
    }
}
