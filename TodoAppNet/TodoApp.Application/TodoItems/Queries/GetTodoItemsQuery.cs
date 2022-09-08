using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.Queries
{
    public class GetTodoItemsQuery : IRequest<List<TodoItemDto>>{ }

    public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, List<TodoItemDto>>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public GetTodoItemsQueryHandler(IRepository<TodoItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            List<TodoItem> todoItems = await _repository.Get(cancellationToken);

            return _mapper.Map<List<TodoItem>, List<TodoItemDto>>(todoItems);
        }   
    }
}
