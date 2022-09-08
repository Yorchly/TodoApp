using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.Commands
{
    public class UpdateTodoItemCommand : IRequest
    {
        public TodoItemDto TodoItemDto { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TodoItem> _repository;

        public UpdateTodoItemCommandHandler(IRepository<TodoItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = _mapper.Map<TodoItem>(request.TodoItemDto);

            await _repository.Update(todoItem, cancellationToken);

            return Unit.Value;
        }
    }
}
