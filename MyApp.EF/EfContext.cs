using Microsoft.EntityFrameworkCore;
using System;

namespace MyApp.EF
{
    public class EfContext : DbContext
    {
        public DbSet<UserGreeting> UserGreetings { get; set; }
    }
}
