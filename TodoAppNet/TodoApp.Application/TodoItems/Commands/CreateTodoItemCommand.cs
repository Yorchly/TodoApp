using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.Commands
{
    public class CreateTodoItemCommand : IRequest<TodoItemDto>
    {
        public CreateTodoItemDto CreateTodoItemDto { get; set; }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TodoItem> _repository;

        public CreateTodoItemCommandHandler(IRepository<TodoItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = _mapper.Map<TodoItem>(request.CreateTodoItemDto);

            todoItem = await _repository.Create(todoItem, cancellationToken);

            return _mapper.Map<TodoItemDto>(todoItem);
        }
    }

}
