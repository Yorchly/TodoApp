using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Contexts;

namespace Data.UnitTests
{
    public class BaseTests
    {
        protected TodoContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new TodoContext(options);

            return dbContext;
        }
            
    }
}
