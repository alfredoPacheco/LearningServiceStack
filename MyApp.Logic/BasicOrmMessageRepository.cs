using MyApp.Model;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyApp.Logic
{
    public class BasicOrmMessageRepository
    : IMessageRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        static List<Message> _messages = new List<Message>();
        public void Add(Message message)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                db.Insert(message);
            }
        }

        public IEnumerable<Message> Where(Expression<Func<Message, bool>> predicate)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return db.Select(predicate);
            }
        }
    }
}
