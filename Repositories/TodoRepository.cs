using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoUygulaması.Data;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(t => t.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllWithCategoriesAsync()
        {
            return await _dbSet.Include(t => t.Category).ToListAsync();
        }
    }
}