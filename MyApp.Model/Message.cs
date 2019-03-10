using ServiceStack;
using System.Collections.Generic;

namespace MyApp.ServiceModel
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

    [Route("/group/{GroupName}")]
    public class Group
    {
        public string GroupName { get; set; }
    }

    public class GroupResponse
    {
        public List<Message> Messages { get; set; }
    }

    [Route("/message/search")]
    [Route("/message/search/{Group}")]
    public class Search
    {
        public string Group { get; set; }
        public string Query { get; set; }
    }
}
