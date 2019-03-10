using ServiceStack;
using MyApp.ServiceModel;
using System.Linq;
using MyApp.ServiceLogic;
using ServiceStack.Data;

namespace MyApp.ServiceInterface
{
    public class MessengerService : Service
    {
        public MyAppSettings ApplicationSettings { get; set; }
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public IMessageRepository MessageRepository { get; set; }

        public MessageResponse Post(Message request)
        {
            MessageRepository.Add(request);
            return new MessageResponse { Response = "OK" };
        }

        public GroupResponse Get(Group request)
        {
            return new GroupResponse
            {
                Messages = MessageRepository
                    .Where(message => message.GroupName == request.GroupName)
                    .ToList()
            };
        }

        [Authenticate]
        public GroupResponse Get(Search request)
        {
            return new GroupResponse
            {
                Messages = MessageRepository
                    .Where(message => message.GroupName == request.Group
                                && message.Body.Contains(request.Query))
                    .ToList()
            };
        }
    }
}
