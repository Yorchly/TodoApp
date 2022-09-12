using Application.UnitTests.Common;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoItems.Commands;
using TodoApp.Domain.Entities;

namespace Application.UnitTests.Queries
{
    public class CreateTodoItemCommandTest : TodoItemCommandQueryCommonTest
    {
        private CreateTodoItemCommand query;
        private CreateTodoItemDto createTodoItemDto;
        private CreateTodoItemCommandHandler handler;
        private const long todoItemCreatedId = 1;

        public CreateTodoItemCommandTest() : base() { }

        [SetUp]
        public void SetUp()
        {
            createTodoItemDto = new CreateTodoItemDto { Content = "Test", Done = true };
            handler = new CreateTodoItemCommandHandler(
                MockRepository.Object, Mapper
            );
            query = new CreateTodoItemCommand
            {
                CreateTodoItemDto = createTodoItemDto
            };
        }

        [Test]
        public async Task Handle_SuccesfullyCreateOnRepository_ReturnsIdOfNewTodoItemCreated()
        {
             MockRepository
                .Setup(repository => repository.Create(It.IsAny<TodoItem>(), new CancellationToken()))
                .ReturnsAsync(todoItemCreatedId);

            long result = await handler.Handle(query, new CancellationToken());

            Assert.That(result, Is.EqualTo(todoItemCreatedId));
        }
    }
}
