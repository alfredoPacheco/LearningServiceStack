using MyApp.Model;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Interface
{
    [Authenticate]
    [RequiredRole("Admin")]
    public class RegisteredUsersService : Service
    {
        public object Get(RegisteredUsers request)
        {
            var response = new
            {
                UserAuths = Db.Select<UserAuth>()
            };

            //nulled out for security
            response.UserAuths.ForEach(x => x.PasswordHash = null);
            response.UserAuths.ForEach(x => x.Salt = null);
            response.UserAuths.ForEach(x => x.DigestHa1Hash = null);

            return response;
        }
    }
}
