using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Data.Contexts
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }

        public void Migrate()
        {
            if (base.Database.GetPendingMigrations().Any())
                base.Database.Migrate();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            base.SaveChangesAsync(cancellationToken);
    }
}
