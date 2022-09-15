using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Data.Contexts;
using TodoApp.Data.Repositories;
using TodoApp.Domain.Entities;

namespace Data.UnitTests
{
    public class RepositoryTests : BaseTests
    {
        private readonly Mock<ILogger<TodoItem>> loggerMock = new();
        private TodoContext firstContext;
        private TodoContext secondContext;

        [SetUp]
        public void SetUp()
        {
            var dbName = Guid.NewGuid().ToString();
            firstContext = BuildContext(dbName);
            secondContext = BuildContext(dbName);
        }

        [TearDown]
        public void TearDown()
        {
            firstContext.Database.EnsureDeleted();
            secondContext.Database.EnsureDeleted();
            firstContext.Dispose();
            secondContext.Dispose();
        }

        #region Create

        [Test]
        public async Task Create_EntityPassedAsArgumentIsValid_EntityIsCreatedAndIsReturned()
        {
            var repository = 
                new TodoItemRepository(firstContext, loggerMock.Object);
            var todoItem = new TodoItem
            {
                Id = 0,
                Content = "Test",
                Done = false
            };

            TodoItem result = await repository.Create(todoItem, new CancellationToken());

            Assert.That(result, Is.Not.Null); 
            TodoItem todoItemFromDb = 
                await secondContext.TodoItems.FindAsync(result.Id);
            Assert.That(todoItemFromDb, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Content, Is.EqualTo(todoItemFromDb.Content));
                Assert.That(result.Done, Is.EqualTo(todoItemFromDb.Done));
            });
        }

        [Test]
        public void Create_EntityIsNull_ThrowsAnArgumentException()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);
            TodoItem todoItem = null;

            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await repository.Create(todoItem, new CancellationToken())
            );
        }

        [Test]
        public void Create_ContextIsNull_ThrowsANullReferenceException()
        {
            var repository =
                new TodoItemRepository(null, loggerMock.Object);
            TodoItem todoItem = null;

            Assert.ThrowsAsync<NullReferenceException>(
                async () => await repository.Create(todoItem, new CancellationToken())
            );
        }

        #endregion

        #region Delete

        [Test]
        public async Task Delete_IdExistsInDatabase_EntityIsDeletedAndAInstanceIsReturned()
        {
            var repository = 
                new TodoItemRepository(firstContext, loggerMock.Object);
            var todoItem = new TodoItem
            {
                Id = 0,
                Content = "Test in delete",
                Done = true,
            };
            await firstContext.TodoItems.AddAsync(todoItem);
            var cancellationToken = new CancellationToken();
            await firstContext.SaveChangesAsync(cancellationToken);
            long deletedTodoItemId = 1;

            TodoItem result = await repository.Delete(deletedTodoItemId, cancellationToken);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(deletedTodoItemId));
                Assert.That(result.Content, Is.EqualTo("Test in delete"));
                Assert.That(result.Done, Is.EqualTo(todoItem.Done));
            });
            TodoItem todoItemFromDb = 
                await secondContext.TodoItems.FindAsync(deletedTodoItemId);
            Assert.That(todoItemFromDb, Is.Null);
        }

        [Test]
        public void Delete_IdIsNotFound_ThrowsANotFoundException()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);

            Assert.ThrowsAsync<NotFoundException>(
                async () => await repository.Delete(1, new CancellationToken())
            );
        }

        [Test]
        public void Delete_ContextIsNull_ThrowsANullReferenceException()
        {
            var repository =
                new TodoItemRepository(null, loggerMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(
                async () => await repository.Delete(1, new CancellationToken())
            );
        }

        #endregion

        #region Get list

        [Test]
        public async Task Get_ReturnsATodoItemsLits()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);
            string todoItem1Content = "Test in get 1";
            string todoItem2Content = "Test in get 2";
            bool todoItem1Done = true;
            bool todoItem2Done = false;
            var todoItems = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 0,
                    Content = todoItem1Content,
                    Done = todoItem1Done,
                },
                new TodoItem
                {
                    Id = 0,
                    Content = todoItem2Content,
                    Done = todoItem2Done,
                }
            };
            await firstContext.TodoItems.AddRangeAsync(todoItems);
            var cancellationToken = new CancellationToken();
            await firstContext.SaveChangesAsync(cancellationToken);

            List<TodoItem> result = await repository.Get(cancellationToken);

            Assert.That(result.Count, Is.EqualTo(2));
            List<TodoItem> todoItemsFromDb = await secondContext.TodoItems.ToListAsync();
            Assert.That(todoItemsFromDb.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Id, Is.EqualTo(todoItemsFromDb[0].Id));
                Assert.That(result[0].Content, Is.EqualTo(todoItemsFromDb[0].Content));
                Assert.That(result[0].Done, Is.EqualTo(todoItemsFromDb[0].Done));
                Assert.That(result[1].Id, Is.EqualTo(todoItemsFromDb[1].Id));
                Assert.That(result[1].Content, Is.EqualTo(todoItemsFromDb[1].Content));
                Assert.That(result[1].Done, Is.EqualTo(todoItemsFromDb[1].Done));
            });
        }

        [Test]
        public void GetList_ContextIsNull_ThrowsANullReferenceException()
        {
            var repository =
                new TodoItemRepository(null, loggerMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(
                async () => await repository.Get(new CancellationToken())
            );
        }

        #endregion

        #region Get

        [Test]
        public async Task Get_ReturnsATodoItem()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);
            var todoItem = new TodoItem
            {
                Id = 0,
                Content = "This is a test from get method",
                Done = true,
            };
            await firstContext.TodoItems.AddAsync(todoItem);
            var cancellationToken = new CancellationToken();
            await firstContext.SaveChangesAsync(cancellationToken);

            TodoItem result = await repository.Get(1, cancellationToken);

            Assert.That(result, Is.Not.Null);
            TodoItem todoItemFromDb = await secondContext.TodoItems.FindAsync((long)1);
            Assert.That(todoItemFromDb, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(todoItemFromDb.Id));
                Assert.That(result.Content, Is.EqualTo(todoItemFromDb.Content));
                Assert.That(result.Done, Is.EqualTo(todoItemFromDb.Done));
            });
        }

        [Test]
        public async Task Get_TodoItemWithIdPassedDoesNotExists_ReturnNull()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);

            TodoItem result = await repository.Get(1, new CancellationToken());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_ContextIsNull_ThrowsANullReferenceException()
        {
            var repository =
                new TodoItemRepository(null, loggerMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(
                async () => await repository.Get(1, new CancellationToken())
            );
        }

        #endregion

        #region SaveChangesAsync

        [Test]
        public async Task SaveChangesAsync_ChangesInEntityPropertiesAreSavedSuccesfully()
        {
            var repository =
                new TodoItemRepository(firstContext, loggerMock.Object);
            var todoItem = new TodoItem
            {
                Id = 0,
                Content = "This is a test from saveChangesAsync method",
                Done = true,
            };
            await firstContext.TodoItems.AddAsync(todoItem);
            var cancellationToken = new CancellationToken();
            await firstContext.SaveChangesAsync(cancellationToken);
            string modifiedContent = "This is a modification test";

            todoItem.Content = modifiedContent;
            await repository.SaveChangesAsync(cancellationToken);

            TodoItem todoItemFromDb = await secondContext.TodoItems.FindAsync((long)1);
            Assert.That(todoItemFromDb, Is.Not.Null);
            Assert.That(todoItemFromDb.Content, Is.EqualTo(modifiedContent));
        }

        [Test]
        public void SaveChangesAsync_ContextIsNull_ThrowsANullReferenceException()
        {
            var repository =
                new TodoItemRepository(null, loggerMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(
                async () => await repository.SaveChangesAsync(new CancellationToken())
            );
        }

        #endregion
    }
}