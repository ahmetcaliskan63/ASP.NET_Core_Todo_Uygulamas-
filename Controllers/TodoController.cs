using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using ToDoUygulaması.Models;
using ToDoUygulaması.Models.ViewModels;
using ToDoUygulaması.Services;

namespace ToDoUygulaması.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly ICategoryService _categoryService;

        public TodoController(ITodoService todoService, ICategoryService categoryService)
        {
            _todoService = todoService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var todos = categoryId.HasValue
                ? await _todoService.GetTodosByCategoryAsync(categoryId.Value)
                : await _todoService.GetAllTodosAsync();

            var categories = await _categoryService.GetAllCategoriesAsync();
            
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.SelectedCategory = categoryId;

            return View(todos);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var viewModel = new TodoViewModel
            {
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var todo = new Todo
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    IsCompleted = viewModel.IsCompleted,
                    DueDate = viewModel.DueDate,
                    CategoryId = viewModel.CategoryId
                };

                await _todoService.AddTodoAsync(todo);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            var viewModel = new TodoViewModel
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedDate = todo.CreatedDate,
                DueDate = todo.DueDate,
                CategoryId = todo.CategoryId,
                Categories = new SelectList(categories, "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var todo = await _todoService.GetTodoByIdAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }

                todo.Title = viewModel.Title;
                todo.Description = viewModel.Description;
                todo.IsCompleted = viewModel.IsCompleted;
                todo.DueDate = viewModel.DueDate;
                todo.CategoryId = viewModel.CategoryId;

                await _todoService.UpdateTodoAsync(todo);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _todoService.DeleteTodoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsCompleted = !todo.IsCompleted;
            await _todoService.UpdateTodoAsync(todo);
            return RedirectToAction(nameof(Index));
        }
    }
}