using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces
{
    public interface ITodoContext
    {
        DbSet<TodoItem> TodoItems { get; set; }
        void Migrate();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
