using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Validators;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoItems.Commands;
using TodoApp.Application.TodoItems.Queries;
using TodoApp.WebApi.Controllers;

namespace Presentation.UnitTests
{
    public class TodoItemControllerTests
    {
        private readonly Mock<IMediator> mediatorMock = new();
        private TodoItemController controller;

        [SetUp]
        public void Setup()
        {
            controller = new TodoItemController(mediatorMock.Object);
        }

        #region GET Tests

        [Test]
        public async Task Get_MediatorGetsTodoItemsDtoListSuccesfully_ReturnsActionResultWithTodoItemsDtoList()
        {
            var todoItemsDto = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Content = "Test", Done = true},
                new TodoItemDto { Id = 2, Content = "Test2", Done = false}
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetTodoItemsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItemsDto);

            ActionResult<List<TodoItemDto>> result = await controller.Get();
            var okObjectResult = result.Result as OkObjectResult;

            OkObjectResultAsserts(okObjectResult);

            var todoItemsDtoFromQuery = okObjectResult.Value as List<TodoItemDto>;

            Assert.That(todoItemsDtoFromQuery, Is.Not.Null);
            Assert.That(todoItemsDtoFromQuery.Count, Is.GreaterThan(0));
            Assert.Multiple(() =>
            {
                Assert.That(todoItemsDtoFromQuery[0].Id, Is.EqualTo(1));
                Assert.That(todoItemsDtoFromQuery[0].Content, Is.EqualTo("Test"));
                Assert.That(todoItemsDtoFromQuery[0].Done, Is.True);
                Assert.That(todoItemsDtoFromQuery[1].Id, Is.EqualTo(2));
                Assert.That(todoItemsDtoFromQuery[1].Content, Is.EqualTo("Test2"));
                Assert.That(todoItemsDtoFromQuery[1].Done, Is.False);
            });
        }

        [Test]
        public async Task Get_MediatorThrowsAnException_ReturnsObjectResultWithStatus500()
        {
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetTodoItemsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(() => new Exception());

            ActionResult<List<TodoItemDto>> result = await controller.Get();

            var objectResult = result.Result as ObjectResult;
            ObjectResultWithStatus500Asserts(objectResult);
        }

        #endregion

        #region POST Tests

        [Test]
        public async Task Post_TodoItemIsCreatedSuccesfully_ReturnsNewTodoItemCreatedId()
        {
            var createTodoItemDto = new CreateTodoItemDto
            {
                Content = "Test",
                Done = true
            };
            var todoItemDto = new TodoItemDto
            {
                Id = 1,
                Content = createTodoItemDto.Content,
                Done = createTodoItemDto.Done
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItemDto);

            ActionResult<TodoItemDto> result = await controller.Post(createTodoItemDto);

            var createdResult = result.Result as CreatedResult;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));

            var createResultTodoItem = createdResult.Value as TodoItemDto;

            Assert.That(createResultTodoItem, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(createResultTodoItem.Id, Is.EqualTo(todoItemDto.Id));
                Assert.That(createResultTodoItem.Content, Is.EqualTo(todoItemDto.Content));
                Assert.That(createResultTodoItem.Done, Is.EqualTo(todoItemDto.Done));
            }
            );
        }

        [Test]
        public async Task Post_MediatorThrowsAnException_ReturnsObjectResultWithStatus500()
        {
            var createTodoItemDto = new CreateTodoItemDto
            {
                Content = "Test",
                Done = true
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .Throws(() => new Exception());

            ActionResult<TodoItemDto> result = await controller.Post(createTodoItemDto);

            var objectResult = result.Result as ObjectResult;
            ObjectResultWithStatus500Asserts(objectResult);
        }

        #endregion

        #region PUT Tests

        [Test]
        public async Task Put_TodoItemIsUpdatedSuccesfully_ReturnsOkResult()
        {
            var todoItemDto = new UpdateTodoItemDto
            {
                Content = "Updated todo test action",
                Done = true
            };
            var updateTodoItemCommand = new UpdateTodoItemCommand
            {
                UpdateTodoItemDto = todoItemDto
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            IActionResult result = await controller.Put(todoItemDto, 1);
            var noContentResult = result as NoContentResult;
            
            Assert.IsNotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Test]
        public async Task Put_ValidationErrorIdIsMinorOrEqualToZero_ReturnsBadRequest()
        {
            var todoItemDto = new UpdateTodoItemDto
            {
                Content = "Test",
                Done = true
            };

            IActionResult result = await controller.Put(todoItemDto, 0);

            var badRequestObjectResult = result as BadRequestObjectResult;
            BadRequestObjectResultAsserts(badRequestObjectResult);
        }

        [Test]
        public async Task Put_TodoItemIsNotFound_ReturnsNotFoundObjectResult()
        {
            var todoItemDto = new UpdateTodoItemDto
            {
                Content = "Updated todo test action",
                Done = true
            };
            var updateTodoItemCommand = new UpdateTodoItemCommand
            {
                UpdateTodoItemDto = todoItemDto
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .Throws(() => new NotFoundException());

            IActionResult result = await controller.Put(todoItemDto, 1);

            var notFoundObjectResult = result as NotFoundObjectResult;
            NotFoundObjectResultAsserts(notFoundObjectResult);
        }

        [Test]
        public async Task Put_MediatorThrowsAnException_ReturnsObjectResultWithStatus500()
        {
            var todoItemDto = new UpdateTodoItemDto
            {
                Content = "Updated todo test action",
                Done = true
            };
            var updateTodoItemCommand = new UpdateTodoItemCommand
            {
                UpdateTodoItemDto = todoItemDto
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .Throws(() => new Exception());

            IActionResult result = await controller.Put(todoItemDto, 1);

            var objectResult = result as ObjectResult;
            ObjectResultWithStatus500Asserts(objectResult);
        }

        #endregion

        #region DELETE Tests

        [Test]
        public async Task Delete_TodoItemIsDeletedSuccesfully_ReturnsOkResult()
        {
            long todoItemToDeleteId = 1;
            var deleteTodoItemCommand = new DeleteTodoItemCommand
            {
                Id = todoItemToDeleteId
            };
            var todoItemDtoDeleted = new TodoItemDto
            {
                Id = todoItemToDeleteId,
                Content = "Test",
                Done = true
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItemDtoDeleted);

            ActionResult<TodoItemDto> result = await controller.Delete(todoItemToDeleteId);

            var okResult = result.Result as OkObjectResult;
            OkObjectResultAsserts(okResult);
        }

        [Test]
        public async Task Delete_ValidationErrorIdIsMinusOrEqualToZero_ReturnsBadRequest()
        {
            long todoItemToDeleteId = 0;

            ActionResult<TodoItemDto> result = await controller.Delete(todoItemToDeleteId);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            BadRequestObjectResultAsserts(badRequestObjectResult);
        }

        [Test]
        public async Task Delete_TodoItemIsNotFound_ReturnsNotFoundObjectResult()
        {
            long todoItemToDeleteId = 1;
            var deleteTodoItemCommand = new DeleteTodoItemCommand
            {
                Id = todoItemToDeleteId
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .Throws(() => new NotFoundException());

            ActionResult<TodoItemDto> result = await controller.Delete(todoItemToDeleteId);

            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            NotFoundObjectResultAsserts(notFoundObjectResult);
        }

        [Test]
        public async Task Delete_MediatorThrowsAnException_ReturnsObjectResultWithStatus500()
        {
            long todoItemToDeleteId = 1;
            var deleteTodoItemCommand = new DeleteTodoItemCommand
            {
                Id = todoItemToDeleteId
            };
            mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .Throws(() => new Exception());

            ActionResult<TodoItemDto> result = await controller.Delete(todoItemToDeleteId);

            var objectResult = result.Result as ObjectResult;
            ObjectResultWithStatus500Asserts(objectResult);
        }

        #endregion

        #region private methods

        private void OkObjectResultAsserts(OkObjectResult okObjectResult)
        {
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        private void BadRequestObjectResultAsserts(BadRequestObjectResult badRequestObjectResult)
        {
            Assert.That(badRequestObjectResult, Is.Not.Null);
            Assert.That(badRequestObjectResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        private void ObjectResultWithStatus500Asserts(ObjectResult objectResult)
        {
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        private void NotFoundObjectResultAsserts(NotFoundObjectResult notFoundObjectResult)
        {
            Assert.That(notFoundObjectResult, Is.Not.Null);
            Assert.That(notFoundObjectResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        #endregion
    }
}