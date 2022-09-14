using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Common.Interfaces
{
    public interface IRepository<T>
    {
        public Task<List<T>> Get(CancellationToken cancellationToken);
        public Task<T> Get(long id, CancellationToken cancellationToken);
        public Task<T> Create(T entity, CancellationToken cancellationToken);
        public Task<T> Delete(long id, CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
