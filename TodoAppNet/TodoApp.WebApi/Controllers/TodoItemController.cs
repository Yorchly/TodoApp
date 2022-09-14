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

        public TodoItemController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<ActionResult<TodoItemDto>> Post([FromBody] CreateTodoItemDto createTodoItemDto)
        {
            try
            {
                TodoItemDto todoItemDto = await _mediator.Send(
                    new CreateTodoItemCommand
                    {
                        CreateTodoItemDto = createTodoItemDto
                    }
                );
                return Created($"TodoItems/{todoItemDto.Id}", todoItemDto);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "TodoItem could not be created");
            }  
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Put([FromBody] UpdateTodoItemDto todoItemDto, long id)
        {
            if (id <= 0)
                return BadRequest("TodoItemDto Id must have a value greater than 0.");

            try
            {
                await _mediator.Send(
                    new UpdateTodoItemCommand 
                    { 
                        UpdateTodoItemDto = todoItemDto, 
                        Id = id 
                    }
                );

                return NoContent();
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

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<TodoItemDto>> Delete(long id)
        {
            if (id <= 0)
                return BadRequest("TodoItemDto Id must have a value greater than 0");

            try
            {
                TodoItemDto result = await _mediator.Send(new DeleteTodoItemCommand { Id = id });

                return Ok(result);
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
