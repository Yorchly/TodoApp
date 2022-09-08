using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoItems.Commands;
using TodoApp.Application.TodoItems.Queries;


namespace TodoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<TodoItemDto> _todoItemDtoValidator;

        public TodoItemController(IMediator mediator, IValidator<TodoItemDto> todoItemDtoValidator)
        {
            _mediator = mediator;
            _todoItemDtoValidator = todoItemDtoValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoItemDto>>> Get()
        {
            try
            {
                List<TodoItemDto> todoItemsDto = await _mediator.Send(new GetTodoItemsQuery());

                return Ok(todoItemsDto);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "TodoItem could not be obtained");
            }
        }
            

        [HttpPost]
        public async Task<ActionResult<long>> Post([FromBody] TodoItemDto todoItemDto)
        {
            ValidationResult validationResult = await _todoItemDtoValidator.ValidateAsync(todoItemDto);

            if (validationResult.Errors.Count > 0)
                return BadRequest(
                    validationResult.Errors.Select(error => error.ErrorMessage)
                );

            try
            {
                long result = await _mediator.Send(
                    new CreateTodoItemCommand
                    {
                        TodoItemDto = todoItemDto
                    }
                );
                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "TodoItem could not be created");
            }  
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Put([FromBody] TodoItemDto todoItemDto)
        {
            var validationResult = await _todoItemDtoValidator.ValidateAsync(
                todoItemDto,
                options => options.IncludeProperties(nameof(todoItemDto.Content), nameof(todoItemDto))
            );

            if (validationResult.Errors.Count > 0)
                return BadRequest(
                    validationResult.Errors.Select(error => error.ErrorMessage)
                );
            if (todoItemDto.Id <= 0)
                return BadRequest("TodoItemDto Id must have a value greater than 0");

            try
            {
                await _mediator.Send(new UpdateTodoItemCommand { TodoItemDto = todoItemDto });

                return Ok();
            }
            catch(NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "TodoItem could not be updated");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(long id)
        {
            if (id <= 0)
                return BadRequest("TodoItemDto Id must have a value greater than 0");

            try
            {
                await _mediator.Send(new DeleteTodoItemCommand { Id = id });

                return Ok();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "TodoItem could not be deleted");
            }
        }
    }
}
