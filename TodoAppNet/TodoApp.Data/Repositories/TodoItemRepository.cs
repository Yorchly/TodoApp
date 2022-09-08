using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Data.Repositories
{
    public class TodoItemRepository : IRepository<TodoItem>
    {
        private readonly ITodoContext _context;
        private readonly ILogger<TodoItem> _logger;

        public TodoItemRepository(ITodoContext context, ILogger<TodoItem> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<long> Create(TodoItem entity, CancellationToken cancellationToken)
        {
            try
            {
                await _context.TodoItems.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating TodoItem");
                throw;
            }
        }

        public async Task Delete(long id, CancellationToken cancellationToken)
        {
            TodoItem todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
                throw new NotFoundException($"TodoItem with Id '{id}' could not be found");

            try
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TodoItemRepository. Todo item could not be deleted");
                throw;
            }
        }

        public async Task<List<TodoItem>> Get(CancellationToken cancellationToken)
        {
            try
            {
                return await _context
                    .TodoItems
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TodoItemRepository. Can get list of todo items.");
                throw;
            }
        }

        public async Task Update(TodoItem entity, CancellationToken cancellationToken)
        {
            TodoItem todoItemFromContext = await _context.TodoItems.FindAsync(entity.Id);

            if (todoItemFromContext == null)
                throw new NotFoundException($"TodoItem with Id '{entity.Id}' could not be found");

            try
            {
                todoItemFromContext.Content = entity.Content;
                todoItemFromContext.Done = entity.Done;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TodoItemRepository. Todo item could not be updated");
                throw;
            }
        }
    }
}
