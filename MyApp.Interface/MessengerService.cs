using MyApp.Logic;
using MyApp.Model;
using ServiceStack;
using System.Linq;

namespace MyApp.Interface
{
    public class MessengerService : Service
    {
        public IMessageRepository MessageRepository { get; set; }

        public object Post(Message request)
        {
            MessageRepository.Add(request);
            return new MessageResponse { Response = "OK" };
        }

        public GroupResponse Get(Group request)
        {
            return new GroupResponse
            {
                Messages = MessageRepository
                        .Where(message => message
                            .GroupName.Equals(request.GroupName))
                        .ToList()
            };
        }
        public GroupResponse Post(Group request)
        {
            MessageRepository.Add(new Message
            {
                Sender = request.Creator,
                GroupName = request.GroupName,
                Body = request.Creator + " created "
                        + request.GroupName + "group."
            });
            return new GroupResponse
            {
                Name = request.GroupName,
                Messages = MessageRepository
                                .Where(message => 
                                        message.GroupName == request.GroupName)
                            .ToList()
            };
        }

        [Authenticate]
        public GroupResponse Get(Search request)
        {
            return new GroupResponse
            {
                Messages = MessageRepository
                        .Where(message => message
                            .GroupName == request.Group
                                && message.Body.Contains(request.Query))
                        .ToList()
            };
        }
    }
}