using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Repositories
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId);
        Task<IEnumerable<Todo>> GetAllWithCategoriesAsync();
    }
}