using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.Commands
{
    public class DeleteTodoItemCommand : IRequest<TodoItemDto>
    {
        public long Id { get; set; }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, TodoItemDto>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public DeleteTodoItemCommandHandler(IRepository<TodoItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = await _repository.Delete(request.Id, cancellationToken);

            return _mapper.Map<TodoItemDto>(todoItem);
        }
    }
}
