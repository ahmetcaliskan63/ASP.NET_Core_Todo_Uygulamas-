using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId);
        Task<Todo> GetTodoByIdAsync(int id);
        Task AddTodoAsync(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(int id);
    }
}