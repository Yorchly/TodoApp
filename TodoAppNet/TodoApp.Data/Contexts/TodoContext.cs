using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Data.Configuration;
using TodoApp.Domain.Entities;

namespace TodoApp.Data.Contexts
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }

        public void Migrate()
        {
           base.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TodoItemConfig());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            base.SaveChangesAsync(cancellationToken);
    }
}
