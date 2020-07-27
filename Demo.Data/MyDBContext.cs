using Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Data
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class MyDBContext:DbContext
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="options"></param>
        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        { }

        /// <summary>
        /// 重写保存方法
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                await this.AddRangeAsync(entry.Entity);
            }
            ChangeTracker.AutoDetectChangesEnabled = false;//代码关闭了变更追踪
            var result = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;//开启变更追踪
            return result;
        }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<UserMain> UserMains { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<RoleMain> RoleMains { get; set; }
    }
}
