using Demo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class MyDBContext:DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        { }

        public DbSet<UserMain> UserMains { get; set; }

        public DbSet<RoleMain> RoleMains { get; set; }
    }
}
