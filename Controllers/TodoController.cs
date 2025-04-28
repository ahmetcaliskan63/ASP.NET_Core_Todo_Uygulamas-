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

        // GET: Todo/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var viewModel = new TodoViewModel
            {
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoViewModel viewModel)
        {
            // ModelState'den Categories alanını çıkar çünkü bu sadece view için kullanılıyor
            ModelState.Remove("Categories");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var todo = new Todo
                    {
                        Title = viewModel.Title,
                        Description = viewModel.Description,
                        IsCompleted = viewModel.IsCompleted,
                        DueDate = viewModel.DueDate,
                        CategoryId = viewModel.CategoryId,
                        CreatedDate = DateTime.Now
                    };

                    await _todoService.AddTodoAsync(todo);
                    
                    // Başarılı mesajını TempData'ya ekle
                    TempData["SuccessMessage"] = "Görev başarıyla kaydedildi!";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata mesajını ModelState'e ekle
                    ModelState.AddModelError("", "Görev eklenirken bir hata oluştu: " + ex.Message);
                }
            }

            // ModelState geçerli değilse veya hata oluşursa, kategorileri yeniden yükle
            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name", viewModel.CategoryId);
            return View(viewModel);
        }

        // GET: Todo/Edit/5
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
                DueDate = todo.DueDate,
                CategoryId = todo.CategoryId,
                CreatedDate = todo.CreatedDate,
                Categories = new SelectList(categories, "Id", "Name", todo.CategoryId)
            };

            return View(viewModel);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            // ModelState'den Categories alanını çıkar çünkü bu sadece view için kullanılıyor
            ModelState.Remove("Categories");

            if (ModelState.IsValid)
            {
                try
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
                    
                    // Başarılı mesajını TempData'ya ekle
                    TempData["SuccessMessage"] = "Görev başarıyla güncellendi!";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata mesajını ModelState'e ekle
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }

            // ModelState geçerli değilse veya hata oluşursa, kategorileri yeniden yükle
            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = new SelectList(categories, "Id", "Name", viewModel.CategoryId);
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