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
            if (environment == EnvironmentEnum.Production)
            {
                var connectionString = "server=mysql;database=todos;user=root;password=secret";
                // User and password MUST NOT be here hardcoded, this is only for testing purposes.
                services.AddDbContext<TodoContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
            }
            else
                services.AddDbContext<TodoContext>(options =>
                    options.UseSqlite("DataSource=TodoAppDatabase.sqlite3")
                );

            services.AddScoped<ITodoContext>(provider => provider.GetService<TodoContext>());
            services.AddScoped<IRepository<TodoItem>, TodoItemRepository>();

            return services;
        }
    }
}
