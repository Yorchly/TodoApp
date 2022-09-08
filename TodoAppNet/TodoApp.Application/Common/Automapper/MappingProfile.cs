using AutoMapper;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<TodoItemDto, TodoItem>();
        }
    }
}
