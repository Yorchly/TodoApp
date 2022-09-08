using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Data.Contexts;
using TodoApp.Data.Repositories;
using TodoApp.Domain.Entities;

namespace TodoApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataProjectDependencies(this IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(options =>
                options.UseSqlite("DataSource=TodoAppDatabase.sqlite3")
            );

            services.AddScoped<ITodoContext>(provider => provider.GetService<TodoContext>());
            services.AddScoped<IRepository<TodoItem>, TodoItemRepository>();

            return services;
        }
    }
}
