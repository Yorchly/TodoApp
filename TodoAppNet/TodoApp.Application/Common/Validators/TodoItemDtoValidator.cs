using FluentValidation;
using TodoApp.Application.Dtos;

namespace TodoApp.Application.Common.Validators
{
    public class TodoItemDtoValidator : AbstractValidator<TodoItemDto>
    {
        public TodoItemDtoValidator()
        {
            RuleFor(todoItemDto => todoItemDto)
                .NotNull()
                .WithMessage("TodoItemDto cannot be null");
            RuleFor(todoItemDto => todoItemDto.Id)
                .Equal(0)
                .WithMessage("TodoItemDto Id cannot be other than 0. It is allowed to no specify the id.");
            RuleFor(TodoItemDto => TodoItemDto.Content)
                .NotEmpty()
                .WithMessage("TodoItemDto content cannot be empty or null");
        }
    }
}
