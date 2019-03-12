using MyApp.Model;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Interface
{
    //public class GreetingService : Service
    //{
    //    public object Any(GetUserGreeting request)
    //    {
    //        var userGreeting = Db.SingleById<UserGreeting>(request.Id);
    //        if (userGreeting == null)
    //        {
    //            throw HttpError.NotFound("Greeting not found");
    //        }

    //        return new GetUserGreetingResponse
    //        {
    //            Result = userGreeting.Greeting.Fmt(request.Name)
    //        };
    //    }
    //}

    public class GreetingUsageService : Service
    {
        public object Any(GetUserGreeting request)
        {
            var userGreeting = Db.LoadSingleById<UserGreeting>(request.Id);
            if (userGreeting == null)
            {
                throw HttpError.NotFound("Greeting not found");
            }

            Db.Insert(new GreetingUsage
            {
                UserGreetingId = userGreeting.Id,
                Language = userGreeting.Language
            });

            return new GetUserGreetingResponse
            {
                Result = userGreeting.Greeting.Fmt(request.Name)
            };
        }
    }
}
