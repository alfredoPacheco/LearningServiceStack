using MyApp.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyApp.ServiceLogic
{
    public class BasicOrmMessageRepository : IMessageRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public void Add(Message message)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                db.Insert(message);
            }
        }

        public IEnumerable<Message> Where(Expression<Func<Message, bool>> expression)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return db.Select(expression);
            }
        }
    }
}
