using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Enums;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Data.Contexts;
using TodoApp.Data.Repositories;
using TodoApp.Domain.Entities;

namespace TodoApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataProjectDependencies(this IServiceCollection services, EnvironmentEnum environment)
        {
            // User and password MUST NOT be here hardcoded, this is only for testing purposes.
            var connectionString = "server=localhost;database=todos;user=root;password=secret";
            services.AddDbContext<TodoContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddScoped<ITodoContext>(provider => provider.GetService<TodoContext>());
            services.AddScoped<IRepository<TodoItem>, TodoItemRepository>();

            return services;
        }
    }
}
