using ServiceStack;
using System.Collections.Generic;

namespace MyApp.Model
{
    [Route("/groups")]
    [Route("/groups/{GroupName}", "GET")]
    public class Group : IReturn<GroupResponse>
    {
        public string GroupName { get; set; }
        public string Creator { get; set; }
    }

    public class GroupResponse
    {
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
    }
}
