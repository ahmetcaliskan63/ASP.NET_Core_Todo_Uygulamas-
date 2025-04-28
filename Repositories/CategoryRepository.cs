using ToDoUygulaması.Data;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}