using System;
using ServiceStack;
using Chapter1.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace Chapter1.ServiceInterface
{
    public class GreetingServices : Service
    {
        public object Get(Greeting request)
        {
            return new GreetingResponse { Result = $"Hello, {request.Name}!" };
        }
    }

    public class MessengerService : Service
    {
        static List<Message> _messages = new List<Message>();
        public MessageResponse Post(Message request)
        {
            _messages.Add(request);
            return new MessageResponse { Response = "OK" };
        }

        public GroupResponse Get(Group request)
        {
            return new GroupResponse
            {
                Messages = _messages.Where(message => message.GroupName.Equals(request.GroupName))
                                    .ToList()
            };
        }

        public GroupResponse Get(Search request)
        {
            return new GroupResponse
            {
                Messages = _messages.Where(
                                message => message.GroupName.Equals(request.Group)
                                && message.Body.Contains(request.Query))
                          .ToList()
            };
        }
    }
}
