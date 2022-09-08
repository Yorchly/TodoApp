using Application.UnitTests.Common;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoItems.Queries;
using TodoApp.Domain.Entities;

namespace Application.UnitTests.Queries
{
    public class GetTodoItemQueryTest : TodoItemCommandQueryCommonTest
    {
        private readonly GetTodoItemsQuery query = new();
        private List<TodoItem> todoItems;
        private GetTodoItemsQueryHandler handler;

        public GetTodoItemQueryTest() : base() { }

        [SetUp]
        public void SetUp()
        {
            handler = new GetTodoItemsQueryHandler(
                MockRepository.Object, Mapper
            );

            todoItems = new List<TodoItem>
            {
                new TodoItem { Id=1, Content="Test", Done=true },
                new TodoItem { Id=2, Content="Test 2", Done=false }
            };
        }

        [Test]
        public async Task Handle_ListIsObtainedFromRepositorySuccesfully_ReturnsListOfTodoItemsDto()
        {
            MockRepository
                .Setup(repository => repository.Get(new CancellationToken()))
                .ReturnsAsync(todoItems);

            List<TodoItemDto> result = await handler.Handle(query, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Content, Is.EqualTo("Test"));
                Assert.That(result[1].Content, Is.EqualTo("Test 2"));
                Assert.That(result[0].Done, Is.True);
                Assert.That(result[1].Done, Is.False);
            });
        }
    }
}
