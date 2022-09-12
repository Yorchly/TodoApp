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
        public UpdateTodoItemDto UpdateTodoItemDto { get; set; }
        public long Id { get; set; }
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
            TodoItem todoItem = await _repository.Get(request.Id, cancellationToken);

            if (todoItem == null)
                return Unit.Value;

            _mapper.Map(request.UpdateTodoItemDto, todoItem);

            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
