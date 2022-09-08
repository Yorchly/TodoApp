using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.Commands
{
    public class DeleteTodoItemCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public DeleteTodoItemCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            await _repository.Delete(request.Id, cancellationToken);

            return Unit.Value;
        }
    }
}
