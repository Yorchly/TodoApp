using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoApp.Application.Common.Automapper;
using TodoApp.Application.Common.Validators;
using TodoApp.Application.Dtos;

namespace TodoApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationProjectDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<TodoItemDto>, TodoItemDtoValidator>();

            return services;
        }
    }
}
