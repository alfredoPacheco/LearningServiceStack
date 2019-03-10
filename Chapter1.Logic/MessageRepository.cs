using Chapter1.ServiceModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chapter1.Logic
{
    public interface IMessageRepository
    {
        void Add(Message message);
        IEnumerable<Message> Where(Func<Message, bool> predicate);
    }

    public class StaticMessageRepository : IMessageRepository
    {
        static List<Message> _messages = new List<Message>();

        public void Add(Message message)
        {
            _messages.Add(message);
        }

        public IEnumerable<Message> Where(Func<Message, bool> predicate)
        {
            return _messages.TakeWhile(predicate).ToList();
        }
    }
}
