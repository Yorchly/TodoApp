using AutoMapper;
using Moq;
using TodoApp.Application.Common.Automapper;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;

namespace Application.UnitTests.Common
{
    public class TodoItemCommandQueryCommonTest
    {
        public Mock<IRepository<TodoItem>> MockRepository { get; set; }
        public IMapper Mapper { get; set; }

        public TodoItemCommandQueryCommonTest()
        {
            MockRepository = new();
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            Mapper = config.CreateMapper();
        }
    }
}
