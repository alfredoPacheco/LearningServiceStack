using MyApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyApp.Logic
{
    public interface IMessageRepository
    {
        void Add(Message message);
        IEnumerable<Message> Where(Expression<Func<Message, bool>> predicate);
    }
    public class StaticMessageRepository
    : IMessageRepository
    {
        static List<Message> _messages = new List<Message>();
        public void Add(Message message)
        {
            _messages.Add(message);
        }
        public IEnumerable<Message> Where(Expression<Func<Message, bool>> predicate)
        {
            return _messages.TakeWhile(predicate.Compile()).ToList();
        }
    }
}
