using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoUygulaması.Models;
using ToDoUygulaması.Repositories;

namespace ToDoUygulaması.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICacheService _cacheService;
        private const string TodoListCacheKey = "TodoList";
        private const string TodoCategoryListCacheKey = "TodoCategoryList_{0}";

        public TodoService(ITodoRepository todoRepository, ICacheService cacheService)
        {
            _todoRepository = todoRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            var cachedTodos = _cacheService.Get<IEnumerable<Todo>>(TodoListCacheKey);
            if (cachedTodos != null)
            {
                return cachedTodos;
            }

            var todos = await _todoRepository.GetAllWithCategoriesAsync();
            _cacheService.Set(TodoListCacheKey, todos, TimeSpan.FromMinutes(5));
            return todos;
        }

        public async Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId)
        {
            var cacheKey = string.Format(TodoCategoryListCacheKey, categoryId);
            var cachedTodos = _cacheService.Get<IEnumerable<Todo>>(cacheKey);
            if (cachedTodos != null)
            {
                return cachedTodos;
            }

            var todos = await _todoRepository.GetTodosByCategoryAsync(categoryId);
            _cacheService.Set(cacheKey, todos, TimeSpan.FromMinutes(5));
            return todos;
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _todoRepository.GetByIdAsync(id);
        }

        public async Task AddTodoAsync(Todo todo)
        {
            await _todoRepository.AddAsync(todo);
            _cacheService.Remove(TodoListCacheKey);
            if (todo.CategoryId.HasValue)
            {
                _cacheService.Remove(string.Format(TodoCategoryListCacheKey, todo.CategoryId.Value));
            }
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            await _todoRepository.UpdateAsync(todo);
            _cacheService.Remove(TodoListCacheKey);
            if (todo.CategoryId.HasValue)
            {
                _cacheService.Remove(string.Format(TodoCategoryListCacheKey, todo.CategoryId.Value));
            }
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo != null)
            {
                await _todoRepository.DeleteAsync(id);
                _cacheService.Remove(TodoListCacheKey);
                if (todo.CategoryId.HasValue)
                {
                    _cacheService.Remove(string.Format(TodoCategoryListCacheKey, todo.CategoryId.Value));
                }
            }
        }
    }
}